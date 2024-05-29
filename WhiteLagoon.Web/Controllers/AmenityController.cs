using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{

    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;   //o nome _unitOfWork é o que quisermos

        //Estamos a aceder através do repositório
        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var amenities = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(amenities);
        }

        //1 - CREATE
        //GET - Este é um clique normal que "mostra" o formulário
        public IActionResult Create()
        {
            AmenityVM amenityVM = new()
            {
                //Estamos a criar um dropdown (um select) mas mostra no Amenity os ids da Villa
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
            };

            return View(amenityVM);
        }

        //POST - Aqui é para gravarmos os dados do formulário
        [HttpPost]  //este método vai receber pelo método post os dados do formulário
        [ValidateAntiForgeryToken]  //para segurança
        public IActionResult Create(AmenityVM obj) 
        {

            if (ModelState.IsValid)
            {

                _unitOfWork.Amenity.Add(obj.Amenity);           //para injectar na bd
                _unitOfWork.Save();                             //vai ver que alterações foram preparadas e faz save na bd

                TempData["success"] = "The Amenity has been created sucessfully!";
                return RedirectToAction(nameof(Index));
            }

            //Dropdown
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            TempData["error"] = "The Amenity could not be created!";
            return View(obj);
        }

        // 2 - EDIT
        //GET - este é para, depois de selecionar qual é aquele que quero editar, recebe o villanumberid
        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(_ => _.Id == amenityId)
            };

            if (amenityVM.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenityVM);
        }

        //POST - para gravar os dados atualizados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(AmenityVM amenityVM)  //metemos VillaNumberVM (porque é o modelo) e a var é o nome que quisermos: villaNumberVM no caso
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(amenityVM.Amenity);    //para injectar na bd
                _unitOfWork.Save();                                     //vai ver que alterações foram preparadas e faz save na bd

                TempData["success"] = "The Amenity has been updated sucessfully!";
                return RedirectToAction(nameof(Index));
            }

            //Se erro, volta para trás e recarregamos o dropdown
            amenityVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            TempData["error"] = "The Amenity could not be updated!";

            return View(amenityVM);
        }

        // 3 - DELETE
        //GET - este é para, depois de selecionar qual é aquele que quero eliminar, recebe o villaNumberId
        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                Amenity = _unitOfWork.Amenity.Get(_ => _.Id == amenityId)
            };

            if (amenityVM.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenityVM);
        }

        //POST - para gravar os dados atualizados depois de ele apagar
        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            Amenity? objFromDb = _unitOfWork.Amenity.Get(_ => _.Id == amenityVM.Amenity.Id);
            if (objFromDb is not null)
            {
                _unitOfWork.Amenity.Remove(objFromDb);
                _unitOfWork.Save();

                TempData["success"] = "The Amenity has been deleted successfully.";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "The Amenity could not be deleted.";
            return View(amenityVM);
        }
    }
}
