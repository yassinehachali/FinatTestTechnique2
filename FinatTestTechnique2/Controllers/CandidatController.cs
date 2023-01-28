using FinatTestTechnique2.Areas.Identity.Data;
using FinatTestTechnique2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace FinatTestTechnique2.Controllers
{
    public class CandidatController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CandidatController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet, HttpPost]
        [Authorize]
        public async Task<IActionResult> Index(string searchByName , string searchByPrenom , string searchByEmail , string searchByTelephone)
        {
            var TheCandidats=await dbContext.Candidats.ToListAsync();
            if (!string.IsNullOrEmpty(searchByName))
                TheCandidats = TheCandidats.Where(e => e.Nom.ToLower().Contains(searchByName.ToLower())).ToList();
            if (!string.IsNullOrEmpty(searchByPrenom))
                TheCandidats = TheCandidats.Where(e => e.Prenom.ToLower().Contains(searchByPrenom.ToLower())).ToList();
            if (!string.IsNullOrEmpty(searchByEmail))
                TheCandidats = TheCandidats.Where(e => e.Mail.ToLower().Contains(searchByEmail.ToLower())).ToList();
            if (!string.IsNullOrEmpty(searchByTelephone))
                TheCandidats = TheCandidats.Where(e => e.Telephone.StartsWith(searchByTelephone)).ToList();
            return View(TheCandidats);
        }

        [HttpGet, HttpPost]
        [Authorize]
        public async Task<IActionResult> View(Guid id)
        {
            var candidate=await dbContext.Candidats.FirstOrDefaultAsync(x=>x.Id==id);
            if (candidate != null) {
                var viewCandidate = new ViewCandidateModel()
                {
                    Id = Guid.NewGuid(),
                    Nom = candidate.Nom,
                    Prenom = candidate.Prenom,
                    Mail = candidate.Mail,
                    Telephone = candidate.Telephone,
                    NiveauEtude = candidate.NiveauEtude,
                    AnneesExperience = candidate.AnneesExperience,
                    DernierEmployeur = candidate.DernierEmployeur,
                };
                return View(viewCandidate);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(ViewCandidateModel model)
        {
            var candidat = await dbContext.Candidats.FindAsync(model.Id);
            if (candidat != null)
            {
                dbContext.Candidats.Remove(candidat);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCv(ViewCandidateModel model)
        {
            var candidat = await dbContext.Candidats.FindAsync(model.Id);
            if (candidat != null)
            {
                var path = Path.Combine("C:\\Users\\HACHA\\Desktop\\Test technique\\Cvs", candidat.CV);
                return File(System.IO.File.ReadAllBytes(path), "application/pdf");
               /* var file = new FileStream(path, FileMode.Open);
                var fileName = candidat.CV;
                var fileType = "application/pdf";
                using (var reader = new BinaryReader(file))
                {
                    var fileContent = reader.ReadBytes((int)file.Length);
                    return File(fileContent, fileType, fileName);
                }*/
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddCandidatViewModel addCandidatureRequest) {
            var candidature = new Candidat()
            {
                Id = Guid.NewGuid(),
                Nom = addCandidatureRequest.Nom,
                Prenom = addCandidatureRequest.Prenom,
                Mail = addCandidatureRequest.Mail,
                Telephone = addCandidatureRequest.Telephone,
                NiveauEtude = addCandidatureRequest.NiveauEtude,
                AnneesExperience = addCandidatureRequest.AnneesExperience,
                DernierEmployeur = addCandidatureRequest.DernierEmployeur,
            };
            var uploads = Path.Combine("C:\\Users\\HACHA\\Desktop\\Test technique", "Cvs");
            if (addCandidatureRequest.CV != null)
            {
                var filePath = Path.Combine(uploads, addCandidatureRequest.CV.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    addCandidatureRequest.CV.CopyTo(fileStream);
                }
                candidature.CV = addCandidatureRequest.CV.FileName;
            }

            await dbContext.Candidats.AddAsync(candidature);
            await dbContext.SaveChangesAsync();
            TempData["Message"] = "Merci Pour Votre Candidature";
            //Mail Sending 
            using (var client = new SmtpClient())
            {
                var message = new MailMessage();
                message.From = new MailAddress("yassinehachali@gmail.com");
                message.To.Add(new MailAddress(addCandidatureRequest.Mail));
                message.Subject = "New Candidature Form Submitted";
                message.Body = "A new candidature form has been submitted.";

                client.UseDefaultCredentials = true;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.Credentials= new NetworkCredential("yassinehachali@gmail.com","nkbvsbtxpuoltzas\r\n");
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(message);
            }
            return RedirectToAction("Add");
        }
    }
}
