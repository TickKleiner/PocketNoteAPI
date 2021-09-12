using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PocketNoteAPI.Models;
using Microsoft.Extensions.Configuration;

// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
namespace PocketNoteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PocketNoteAPIItemsController : ControllerBase
    {
        private PocketNoteAPIContext _context;

        public PocketNoteAPIItemsController(PocketNoteAPIContext context)
        {
            _context = context;
        }

        // Добавляет пользователя в бд
        // POST: api/PocketNoteAPIItems/SignUp
        [HttpPost("SignUp")]
        public async Task<ActionResult> PostSignUp([FromBody] PostSignUpDataDTO postSignUpDataDTO)
        {
            var pocketNoteAPIItem = new PocketNoteAPIItem(postSignUpDataDTO);
            await _context.PocketNoteAPIItems.AddAsync(pocketNoteAPIItem);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // Существует ли пользователь
        // GET: api/PocketNoteAPIItems/UserExist?GoogleUserId=
        [HttpHead("UserExist")]
        public async Task<ActionResult> GetUserExist(string GoogleUserId)
        {
            var pocketNoteAPIItem = await _context.PocketNoteAPIItems.FindAsync(GoogleUserId);
            if (pocketNoteAPIItem == null)
            {
                return NotFound();
            }
            return Ok();
        }

        // Отправляю девайс и получаю токен авторизации
        // GET: api/PocketNoteAPIItems/Auth?GoogleUserId=
        [HttpPatch("Auth")]
        public async Task<ActionResult<GetAuthTokenDTO>> PatchAuth(string GoogleUserId, PostAuthDataDTO authDataDTO)
        {
            var pocketNoteAPIItem = await _context.PocketNoteAPIItems.FindAsync(GoogleUserId);
            if (pocketNoteAPIItem == null)
            {
                return NotFound();
            }

            Device device = await _context.Devices.FindAsync(authDataDTO.DeviceId);
            if (device == null)
            {
                device = new Device(authDataDTO.DeviceId);
                device.GoogleUserId = GoogleUserId;
                device.PocketNoteAPIItem = pocketNoteAPIItem;
                await _context.Devices.AddAsync(device);
            }

            Session session = await _context.Sessions.FindAsync(authDataDTO.DeviceId, GoogleUserId);
            if (session == null)
            {
                session = new Session(authDataDTO.DeviceId, GoogleUserId);
                session.Device = device;
                session.UpdateSession(authDataDTO.AuthDate, authDataDTO.Ip);
                await _context.Sessions.AddAsync(session);
            } else
            {
                session.UpdateSession(authDataDTO.AuthDate, authDataDTO.Ip);
            }

            GetAuthTokenDTO getAuthTokenDTO = new GetAuthTokenDTO();
            pocketNoteAPIItem.AuthToken = getAuthTokenDTO.AuthToken;
            await _context.SaveChangesAsync();
            return getAuthTokenDTO;
        }

        // Разлогиниваюсь
        // GET: api/PocketNoteAPIItems/{GoogleUserId}/SignOut?AuthToken=
        [HttpPatch("{GoogleUserId}/SignOut")]
        public async Task<ActionResult> PatchSignOut(string GoogleUserId, string AuthToken)
        {
            var pocketNoteAPIItem = await _context.PocketNoteAPIItems.FindAsync(GoogleUserId);
            if (pocketNoteAPIItem == null)
            {
                return NotFound();
            }
            if (pocketNoteAPIItem.AuthToken == null || AuthToken != pocketNoteAPIItem.AuthToken)
            {
                return BadRequest();
            }

            pocketNoteAPIItem.AuthToken = null;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // Получить данные после получения токена(пин и девайсы)
        // GET: api/PocketNoteAPIItems/{GoogleUserId}/AppData?AuthToken=
        [HttpGet("{GoogleUserId}/AppData")]
        public async Task<ActionResult<GetAppDataDTO>> GetAppData(string GoogleUserId, string AuthToken)
        {
            var pocketNoteAPIItem = await _context.PocketNoteAPIItems.FindAsync(GoogleUserId);
            if (pocketNoteAPIItem == null)
            {
                return NotFound();
            }
            if (pocketNoteAPIItem.AuthToken == null || AuthToken != pocketNoteAPIItem.AuthToken)
            {
                return BadRequest();
            }
            GetAppDataDTO response = new GetAppDataDTO(pocketNoteAPIItem);
            response.Sessions = await _context.Sessions.Where(x => x.GoogleUserId == GoogleUserId).ToListAsync();
            return response;
        }

        // Получить данные о загруженных файлах
        // GET: api/PocketNoteAPIItems/FileStorage/{GoogleUserId}/DownloadData?AuthToken=
        [HttpGet("FileStorage/{GoogleUserId}/DownloadData")]
        public async Task<ActionResult<GetDownloadDataDTO>> GetDownloadData(string GoogleUserId, string AuthToken)
        {
            PocketNoteAPIItem pocketNoteAPIItem = await _context.PocketNoteAPIItems.FindAsync(GoogleUserId);
            if (pocketNoteAPIItem == null)
            {
                return NotFound();
            }
            if (
                String.IsNullOrEmpty(AuthToken) ||
                String.IsNullOrEmpty(pocketNoteAPIItem.AuthToken) ||
                AuthToken != pocketNoteAPIItem.AuthToken
            )
            {
                return BadRequest();
            }
            GetDownloadDataDTO response = new GetDownloadDataDTO(pocketNoteAPIItem);
            response.FilesToFileDTO(await _context.Files.Where(x => x.GoogleUserId == GoogleUserId).ToListAsync());
            return response;
        }

        // Загрузить информацию о загруженных файлах
        // POST: api/PocketNoteAPIItems/UploadFiles/{GoogleUserId}/PostFiles?AuthToken=
        [HttpPost("FileStorage/{GoogleUserId}/PostFiles")]
        public async Task<ActionResult> PostFiles(string GoogleUserId, string AuthToken, [FromBody] PostFilesDTO postFilesDTO)
        {
            var pocketNoteAPIItem = await _context.PocketNoteAPIItems.FindAsync(GoogleUserId);
            if (pocketNoteAPIItem == null)
            {
                return NotFound();
            }
            if (pocketNoteAPIItem.AuthToken == null || AuthToken != pocketNoteAPIItem.AuthToken)
            {
                return BadRequest();
            }
            foreach (FileDTO fileDTO in postFilesDTO.Files)
            {
                File file = await _context.Files.FirstOrDefaultAsync(x => x.FileId == fileDTO.FileId);
                if (file == null)
                {
                    file = new File(fileDTO.FileId, GoogleUserId);
                    file.PocketNoteAPIItem = pocketNoteAPIItem;
                    file.update_data(fileDTO);
                    await _context.Files.AddAsync(file);
                } else
                {
                    file.update_data(fileDTO);
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        // Обновить информацию о загруженных файлах
        // POST: api/PocketNoteAPIItems/UploadFiles/{GoogleUserId}/PatchFiles?AuthToken=
        [HttpPatch("FileStorage/{GoogleUserId}/PatchFiles")]
        public async Task<ActionResult> PatchFiles(string GoogleUserId, string AuthToken, [FromBody] PatchFilesDTO patchFilesDTO)
        {
            var pocketNoteAPIItem = await _context.PocketNoteAPIItems.FindAsync(GoogleUserId);
            if (pocketNoteAPIItem == null)
            {
                return NotFound();
            }
            if (pocketNoteAPIItem.AuthToken == null || AuthToken != pocketNoteAPIItem.AuthToken)
            {
                return BadRequest();
            }
            var set = new HashSet<string>(patchFilesDTO.FileIDs);
            List<File> Files =  await _context.Files
                                .Where(x => x.GoogleUserId == GoogleUserId)
                                .Where(x => !set.Contains(x.FileId))
                                .ToListAsync();
            _context.Files.RemoveRange(Files);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
