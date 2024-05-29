using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers
{

    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;   //o nome _unitOfWork é o que quisermos

        //para as imagens
        private readonly IWebHostEnvironment _webHostEnvironment;

        //CONSTRUTOR
        //Estamos a aceder através do repositório
        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        //INDEX
        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();

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
                //File Upload
                if (obj.Image is not null)
                {
                    //Devemos gerar sempre nomes aleatórios na bd para que ela não estoire cada vez que o user faz upload de uma
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    //Guid é o que garante o nome igual. Defini aqui o nome para a imagem
                    //Preciso também da extensão da imagem. Esse comando é o que esta a seguir ao +. 
                    //onde vou guardar a imagem?
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImages");
                    //(vai dar ao caminho até à nossa pasta: www.root + o caminho para as pastas)

                    //proteção para a bd não estoirar
                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        obj.Image.CopyTo(fileStream);
                    }
                    obj.ImageUrl = @"\images\VillaImages\" + fileName;
                }
                else
                {
                    //Se o user não colocar imagem, é colocado esta por default
                    obj.ImageUrl = "https://placehold.co/600x400";
                }


                _unitOfWork.Villa.Add(obj);    //para injectar na bd
                _unitOfWork.Save();            //vai ver que alterações foram preparadas e faz save na bd

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
            Villa? obj = _unitOfWork.Villa.Get(x => x.Id == villaId);

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
				//File Upload
				if (obj.Image is not null)
				{
					string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
					string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImages");
                        
                    //Se existir imagem, o objetivo é apagá-la antes de colocar a nova
                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
                        //Para tirar catateres especiais do path, usamos o TrimStart.
                       
                        //Se o ficheiro existir, vamos apagar a imagem
                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
					}
					//proteção para a bd não estoirar
					using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
					{
						obj.Image.CopyTo(fileStream);
					}
					obj.ImageUrl = @"\images\VillaImages\" + fileName;
				}

				_unitOfWork.Villa.Update(obj);         //para injectar na bd c/o update
                _unitOfWork.Save();                   //vai ver que alterações foram preparadas e faz save na bd

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
            Villa? obj = _unitOfWork.Villa.Get(x => x.Id == villaId); 

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
            Villa? objFromDb = _unitOfWork.Villa.Get(_ => _.Id == obj.Id);

            if (objFromDb is not null)
            {
				//Se existir imagem, o objetivo é apagá-la antes de colocar a nova
				if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
				{
					var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));
					//Para tirar catateres especiais do path, usamos o TrimStart.

					//Se o ficheiro existir, vamos apagar a imagem
					if (System.IO.File.Exists(oldImagePath))
						System.IO.File.Delete(oldImagePath);
				}
				_unitOfWork.Villa.Remove(objFromDb);
                _unitOfWork.Save();              //vai ver que alterações foram preparadas e faz save na bd

                TempData["success"] = "The Villa has been deleted sucessfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "The Villa could not be deleted!";
            return View(obj);
        }
    }
}
