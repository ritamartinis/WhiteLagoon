using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

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
            var villasNumbers = _db.VillaNumbers.ToList();

            return View(villasNumbers);
        }

        //1 - CREATE

        //GET - Este é um clique normal que "mostra" o formulário
        public IActionResult Create()
        {
            return View();
        }

        //POST - Aqui é para gravarmos os dados do formulário
        [HttpPost]  //este método vai receber pelo método post os dados do formulário
        [ValidateAntiForgeryToken]  //para segurança
        public IActionResult Create(VillaNumber obj)  //metemos Villa (porque é o modelo) e a var é o nome que quisermos: obj no caso
        {

            if (ModelState.IsValid)     //validações do lado do servidor
            {
                _db.VillaNumbers.Add(obj);    //para injectar na bd
                _db.SaveChanges();      //vai ver que alterações foram preparadas e faz save na bd

                TempData["success"] = "The Villa has been created sucessfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "The Villa could not be created!";
            return View(obj);   
        }

        // 2 - EDIT

        //GET - este é para, depois de selecionar qual é aquele que quero editar, recebe o id 
        public IActionResult Update(int villaId)
        {
            //Aqui estou a aceder à bd, a comparar com o id que foi selecionado pelo clique do get
            Villa? obj = _db.Villas.FirstOrDefault(x => x.Id == villaId);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");   //1º o ficheiro, depois a vista. É o error, da vista Home. Temos de indicar o caminho.
            }
            return View(obj);   //se não for nulo, devolvo TODOS os dados da Villa que quero abrir e editar
        }

        //POST - para gravar os dados atualizados
        [HttpPost]  
        [ValidateAntiForgeryToken]  
        public IActionResult Update(Villa obj)  //metemos Villa (porque é o modelo) e a var é o nome que quisermos: obj no caso
        { 
            if (ModelState.IsValid && obj.Id > 0)             //validações do lado do servidor
                                                              //segurança adicional - se o Id for zero, nunca dá update. Zero é para criar.
            {
                _db.Villas.Update(obj);         //para injectar na bd c/o update
                _db.SaveChanges();              //vai ver que alterações foram preparadas e faz save na bd

                TempData["success"] = "The Villa has been updated sucessfully!";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa could not be updated!";
            return View(obj);
        }

        // 3 - DELETE

        //GET - este é para, depois de selecionar qual é aquele que quero eliminar, recebe o id 
        public IActionResult Delete(int villaId)
        {
            //Aqui estou a aceder à bd, a comparar com o id que foi selecionado pelo clique do get
            Villa? obj = _db.Villas.FirstOrDefault(x => x.Id == villaId);

            if (obj == null)
            {
                return RedirectToAction("Error", "Home");   //1º o ficheiro, depois a vista. É o error, da vista Home. Temos de indicar o caminho.
            }

            return View(obj);   //se não for nulo, devolvo TODOS os dados da Villa que quero abrir e editar
        }

        //POST - para gravar os dados atualizados depois de ele apagar
        [HttpPost]
        public IActionResult Delete(Villa obj)  //metemos Villa (porque é o modelo) e a var é o nome que quisermos: obj no caso
        {
            Villa? objFromDb = _db.Villas.FirstOrDefault(_ => _.Id == obj.Id);

            if (objFromDb is not null)
            {
                _db.Villas.Remove(objFromDb);
                _db.SaveChanges();              //vai ver que alterações foram preparadas e faz save na bd

                TempData["success"] = "The Villa has been deleted sucessfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "The Villa could not be deleted!";
            return View(obj);
        }
    }
}
