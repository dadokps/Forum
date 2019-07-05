using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using LambdaForums.Data.Models;
using LambdaForums.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using LambdaForum.Models.ApplicationUser;
using Microsoft.AspNetCore.Hosting;

namespace LambdaForum.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        //private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;
        private IHostingEnvironment _hostingEnvironment;

        public ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _userService = userService;
            //_uploadService = uploadService;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize]
        public IActionResult Detail(string id)
        {
            var user = _userService.GetById(id);
            var userRoles = _userManager.GetRolesAsync(user).Result;

            var model = new ProfileModel()
            {
                UserId = user.Id,
                Username = user.UserName,
                UserRating = user.Rating.ToString(),
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                DateJoined = user.MemberSince,
                IsActive = user.IsActive,
                IsAdmin = userRoles.Contains("Admin")
            };

            return View(model);
        }

        /*
        Uploads the Image of the User. Stores in images Folder and writes the path in DB
        */
        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = _userManager.GetUserId(User);
            string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            var fileUri = filePath + "\\" + file.FileName;
            var inImagesPath = "/images/" + file.FileName;
            if (file.Length > 0)
            {
                using (var stream = new FileStream(fileUri, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    await _userService.SetProfileImage(userId, inImagesPath);
                }
            }
          
            return RedirectToAction("Detail", "Profile", new { id = userId });
        }
        /*
         * Files uploaded using the IFormFile technique are buffered in memory or on disk on the web server 
         * before being processed. Inside the action method, the IFormFile contents are accessible as a stream. 
         * In addition to the local file system, files can be streamed to Azure Blob storage or Entity Framework.
         

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = _userManager.GetUserId(User);
            var connectionString = _configuration.GetConnectionString("AzureStorageAccountConnectionString");
            var container = _uploadService.GetBlobContainer(connectionString);

            var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var filename = Path.Combine(parsedContentDisposition.FileName.Trim('"'));

            var blockBlob = container.GetBlockBlobReference(filename);

            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
            await _userService.SetProfileImage(userId, blockBlob.Uri);

            return RedirectToAction("Detail", "Profile", new { id = userId });
        }
        */

        public IActionResult Index()
        {
            var profiles = _userService.GetAll()
                .OrderByDescending(user => user.Rating)
                .Select(u => new ProfileModel
                {
                    Email = u.Email,
                    ProfileImageUrl = u.ProfileImageUrl,
                    UserRating = u.Rating.ToString(),
                    DateJoined = u.MemberSince,
                    IsActive = u.IsActive
                });

            var model = new ProfileListModel
            {
                Profiles = profiles
            };

            return View(model);
        }

        public IActionResult Deactivate(string userId)
        {
            var user = _userService.GetById(userId);
            _userService.Deactivate(user);
            return RedirectToAction("Index", "Profile");
        }
    }
}