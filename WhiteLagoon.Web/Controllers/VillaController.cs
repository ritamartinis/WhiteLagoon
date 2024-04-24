using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{

    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();

            return View(villas);
        }

        //1 - CREATE A NEW VILLA

        //GET - Este é um clique normal que "mostra" o formulário
        public IActionResult Create()
        {
            return View();
        }

        //POST - Aqui é para gravarmos os dados do formulário
        [HttpPost]  //este método vai receber pelo método post os dados do formulário
        [ValidateAntiForgeryToken]  //para segurança
        public IActionResult Create(Villa obj)  //metemos Villa (porque é o modelo) e a var é o nome que quisermos: obj no caso
        {
            //Validações personalizadas - que o modelo não prevê
            //Fazemos essas mesmo antes de ver se o modelo está válido ou não, ie, antes da linha 42
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Name", "The Description cannot be the same as the Name"); 
            }

            if (ModelState.IsValid)     //validações do lado do servidor
            {
                _db.Villas.Add(obj);    //para injectar na bd
                _db.SaveChanges();      //vai ver que alterações foram preparadas e faz save na bd
                return RedirectToAction(nameof(Index));
            }
        return View(obj);   
        }

        // 2 - EDIT

        //GET - este é para, depois de selecionar qual é aquele que quero editar, recebe o id 
        public IActionResult Update(int villaId)
        {
            //Aqui estou a aceder à bd, a comparar com o id que foi selecionado pelo clique do get
            Villa? obj = _db.Villas.FirstOrDefault(x => x.Id == villaId);

            if (obj == null)
            {
                return NotFound();  
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
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}
