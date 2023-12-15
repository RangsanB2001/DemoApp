using AuthenticationAPI.Models;
using AuthenticationModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly RangsanContext _dbContext;

        public UserController(RangsanContext dbContext)
        {
            _dbContext = dbContext;
        }
        // สมัครสมาชิก
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] AddUserRequest request, CancellationToken cancellationToken = default)
        {

            /* EncryptionService encryptionService = new EncryptionService("=EunN/CgBs_EUO9FNYiRR6c:QDr-AY9_");

             // เข้ารหัสรหัสผ่านโดยใช้วิธี AES encryption
             string encryptedPassword = encryptionService.EncryptPassword(new AddUserRequest { Password = request.Password });*/

            var user = new Models.User
            {
                username = request.UserName,
                password = request.Password, // นำรหัสที่เข้ารหัสแล้วไปเก็บในฐานข้อมูล
            };

            await _dbContext.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok(user);
        }
        // ล็อกอิน
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
        {

            /*          EncryptionService encryptionService = new EncryptionService("=EunN/CgBs_EUO9FNYiRR6c:QDr-AY9_");

                      // เข้ารหัสรหัสผ่านโดยใช้วิธี AES encryption
                      string decryptedPassword = encryptionService.DecryptPassword(new LoginRequest { Password = request.Password });*/

            var query = await _dbContext.users
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                x =>
                x.username == request.Username &&
                x.password == request.Password
                , cancellationToken);

            if (query != null)
            {
                var returnData = new LoginResponse();
                returnData.iduser = query.iduser;
                returnData.username = query.username;
                return Ok(returnData);
            }
            return BadRequest();
        }
        // อัปเดต
        [HttpPatch("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] Models.User request, CancellationToken cancellationToken = default)
        {

            var FindUsers = await _dbContext.users.FirstOrDefaultAsync(u => u.iduser == request.iduser);

            if (FindUsers != null)
            {
                if (!string.IsNullOrWhiteSpace(request.username))
                {
                    FindUsers.username = request.username;
                }

                if (!string.IsNullOrWhiteSpace(request.password))
                {
                    FindUsers.password = request.password;
                }

                _dbContext.Update(FindUsers);
                var Updated = await _dbContext.SaveChangesAsync(cancellationToken);

                if (Updated > 0)
                {
                    // Retrieve the updated user from the database
                    var updatedUser = await _dbContext.users
                        .FirstOrDefaultAsync(u => u.iduser == FindUsers.iduser, cancellationToken);

                    if (updatedUser != null)
                    {
                        var returnData = new GetDataUser
                        {
                            iduser = updatedUser.iduser,
                            username = updatedUser.username,
                            password = updatedUser.password
                        };

                        return Ok(returnData);
                    }
                }
            }
            return NotFound();
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int iduser, CancellationToken cancellationToken = default)
        {
            var userToDelete = await _dbContext.users.FirstOrDefaultAsync(x => x.iduser == iduser, cancellationToken);

            if (userToDelete == null)
            {
                // User not found
                return NotFound();
            }

            _dbContext.Remove(userToDelete);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var returnData = new GetDataUser
            {
                iduser = userToDelete.iduser,
                username = userToDelete.username,
            };
            return Ok(returnData);
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> GetUserProfile(int iduser, CancellationToken cancellationToken = default)
        {
            var userData = await _dbContext.users
                 .AsSplitQuery()
                 .AsNoTracking()
                 .FirstOrDefaultAsync(x => x.iduser == iduser, cancellationToken);

            if (userData != null)
            {
                var returnData = new GetDataUser();
                returnData.iduser = userData.iduser;
                returnData.username = userData.username;
                returnData.password = userData.password;
                return Ok(returnData);
            }
            return NotFound();
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData(int iduser, CancellationToken cancellationToken = default)
        {

            var usersData = await _dbContext.users
                .AsSplitQuery()
                .AsNoTracking()
                .Where(x => x.iduser != iduser)
                .ToListAsync(cancellationToken);

            if (usersData != null && usersData.Any())
            {
                var returnDataList = usersData.Select(userData => new GetDataUser
                {
                    iduser = userData.iduser,
                    username = userData.username,
                    password = userData.password
                }).ToList();

                return Ok(returnDataList);
            }

            return NotFound();
        }

    }
}
