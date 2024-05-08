using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{

    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<VillaNumber>? villasNumbers = _db.VillaNumbers
                //Estamos a incluir os dados da Villa.cs
                .Include(u => u.Villa)
                .ToList();

            return View(villasNumbers);
        }

        //1 - CREATE

        //GET - Este é um clique normal que "mostra" o formulário
        public IActionResult Create()
        {
            //VillaNumberVM é um novo modelo - que só é usado nas vistas - que é superior aos outros
            VillaNumberVM villaNumberVM = new()
            {
                //Estamos a criar um dropdown (um select) mas mostra no VillaNumber os ids da Villa
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
            };

            return View(villaNumberVM);
        }

        //POST - Aqui é para gravarmos os dados do formulário
        [HttpPost]  //este método vai receber pelo método post os dados do formulário
        [ValidateAntiForgeryToken]  //para segurança
        public IActionResult Create(VillaNumberVM obj) 
        {

            if (ModelState.IsValid)
            {
                //Caso o VillaNumber já exista na bd
                if (_db.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number))
                {
                    TempData["error"] = "The Villa number already exists!";
                    //Caso dê erro, voltamos a carregar o dropdown do get do create para que os dados possam aparecer de novo.
                    obj.VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString(),
                    });
                    return View(obj);           //volta aqui para o utilizador poder corrigir
                }

                _db.VillaNumbers.Add(obj.VillaNumber);    //para injectar na bd
                _db.SaveChanges();                        //vai ver que alterações foram preparadas e faz save na bd

                TempData["success"] = "The Villa has been created sucessfully!";
                return RedirectToAction(nameof(Index));
            }

            //Resolvi pôr aqui também, para outro eventual erro.
            obj.VillaList = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            TempData["error"] = "The Villa could not be created!";
            return View(obj);
        }

        // 2 - EDIT

        //GET - este é para, depois de selecionar qual é aquele que quero editar, recebe o villanumberid
        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)!
            };
            
            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumberVM);
        }

        //POST - para gravar os dados atualizados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(VillaNumberVM villaNumberVM)  //metemos VillaNumberVM (porque é o modelo) e a var é o nome que quisermos: villaNumberVM no caso
        {
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(villaNumberVM.VillaNumber);    //para injectar na bd
                _db.SaveChanges();                                     //vai ver que alterações foram preparadas e faz save na bd

                TempData["success"] = "The Villa has been updated sucessfully!";
                return RedirectToAction(nameof(Index));
            }

            //Se erro, volta para trás e recarregamos o dropdown
            villaNumberVM.VillaList = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            TempData["error"] = "The Villa could not be updated!";

            return View(villaNumberVM);
        }

        // 3 - DELETE

        //GET - este é para, depois de selecionar qual é aquele que quero eliminar, recebe o villaNumberId
        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)!
            };

            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumberVM);
        }

        //POST - para gravar os dados atualizados depois de ele apagar
        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = _db.VillaNumbers.FirstOrDefault(_ => _.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if (objFromDb is not null)
            {
                _db.VillaNumbers.Remove(objFromDb);
                _db.SaveChanges();

                TempData["success"] = "The Villa has been deleted successfully.";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "The Villa could not be deleted.";
            return View(villaNumberVM);
        }
    }
}
