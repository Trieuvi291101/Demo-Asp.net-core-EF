using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using testNetCoreWebApp.Data;
using testNetCoreWebApp.Models;
using Microsoft.AspNetCore.Http;
using  Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Extensions;
using testNetCoreWebApp.Helpers;
using System.Transactions;

namespace testNetCoreWebApp.Controllers
{
    public class ProductsController : Controller
    {
        
        private readonly saledbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductsController(saledbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index(string kw)
        {
            
            var saledbContext = _context.Product.Include(p => p.Category);

            if (!String.IsNullOrEmpty(kw))
            {
                var search = saledbContext.Select(s => s);
                search = search.Where(s => s.Name == kw);
                return View(search);
            }

            return View(await saledbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Manufacturer,CreatedDate,CategoryId,ThumbnailImage")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (_context.Product.Select(s => s.Id).Count() == 0)
                {
                    product.Id = 1;
                }
                else
                {
                    int p = _context.Product.Select(s => s.Id).Max();
                    product.Id = p;
                    product.Id++;
                }


                product.CreatedDate = DateTime.Today;
                if (product.ThumbnailImage != null)
                {
                    //upload ảnh sp
                    string wwwrootPath = _hostEnvironment.WebRootPath;
                    string filename = Path.GetFileNameWithoutExtension(product.ThumbnailImage.FileName);
                    string extention = Path.GetExtension(product.ThumbnailImage.FileName);
                    product.Image = filename = filename + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhh")) + extention;
                    string path = Path.Combine(wwwrootPath + "/image/", filename);

                    using (var file = new FileStream(path, FileMode.Create))
                    {
                        await product.ThumbnailImage.CopyToAsync(file);

                    }
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            string errors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            ModelState.AddModelError("", errors);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Manufacturer,ThumbnailImage,CreatedDate,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.CreatedDate = DateTime.Today;
                    if (product.ThumbnailImage != null)
                    {
                        //upload ảnh sp
                        string wwwrootPath = _hostEnvironment.WebRootPath;
                        string filename = Path.GetFileNameWithoutExtension(product.ThumbnailImage.FileName);
                        string extention = Path.GetExtension(product.ThumbnailImage.FileName);
                        product.Image = filename = filename + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhh")) + extention;
                        string path = Path.Combine(wwwrootPath + "/image/", filename);

                        using (var file = new FileStream(path, FileMode.Create))
                        {
                            await product.ThumbnailImage.CopyToAsync(file);

                        }
                    }
                    else

                        _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            while (_context.OrderDetail.Any(u => u.ProductId == id))
            {
                var o = _context.OrderDetail.FirstOrDefault(s => s.ProductId == id);
                var t = o.Id;
                var or = _context.OrderDetail.FirstOrDefault(s => s.Id == t);
                _context.OrderDetail.Remove(or);
                await _context.SaveChangesAsync();


            }

            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Store(string kw)
        {
            var saledbContext = _context.Product.Include(p => p.Category);

            if (!String.IsNullOrEmpty(kw))
            {
                var search = saledbContext.Select(s => s);
                search = search.Where(s => s.Name == kw);
                return View(search);
            }

            return View(await saledbContext.ToListAsync());
        }
         public List<ItemCart> Carts
		{
			get
			{
                var cart = HttpContext.Session.Get<List<ItemCart>>("gioHang");
				if (cart == default(List<ItemCart>))
				{
					cart = new List<ItemCart>();
				}
				return cart;
			}

		}
        public async Task<IActionResult> CartIndex()
		{
            ViewData["Null"] = "không có sản phẩm nào!!!";
            ViewData["countCart"] = countCart();
            ViewData["totalCart"] = totalCart();
            return View(Carts);
		}
		public async Task<IActionResult> AddToCart(int id , int quantity)
        {
            var myCart = Carts;

            var item = myCart.SingleOrDefault(s => s.idCart == id);
            if(item == null)
			{
                var pro = _context.Product.SingleOrDefault(p => p.Id == id);
                item = new ItemCart
                {
                    idCart = id,
                    NameCart = pro.Name,
                    Image = pro.Image,
                    priceCart = pro.Price,
                    quantity = quantity
                };
                myCart.Add(item);
			}
			else
			{
                item.quantity += quantity;
			}
            HttpContext.Session.Set("gioHang", myCart);

            return RedirectToAction("CartIndex");
        }
        public ActionResult ClearCart()
        {
            List<ItemCart> gioHang = Carts;
            HttpContext.Session.Remove("gioHang");
            return RedirectToAction("CartIndex");
        }

        public int countCart()
		{
            int n = 0;
            var data = HttpContext.Session.Get<List<ItemCart>>("gioHang");
            if(data != null)
			{
                n = data.Sum(s => s.quantity);
			}
            return n;
        }
        public decimal totalCart()
		{
            decimal d = 0;
            var data = HttpContext.Session.Get<List<ItemCart>>("gioHang");
            if (data != null)
            {
                d = (decimal)data.Sum(s => s.total);
            }
            return d;
        }

        public ActionResult RemoveCart(int id)
		{
            List<ItemCart> gioHang = Carts;
            ItemCart item = gioHang.Find(p => p.idCart == id);
            
            if (item != null)
			{
                gioHang.RemoveAll(s => s.idCart==id);
			}
            HttpContext.Session.Set("gioHang", gioHang);
            return RedirectToAction("CartIndex");
        }
		private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        public ActionResult OrderProduct()
        {
            using (TransactionScope tranScope = new TransactionScope())
            {
                try
                {
                    SaleOrder order = new SaleOrder();
                    if (_context.SaleOrder.Select(s => s.Id).Count() == 0)
                    {
                        order.Id = 1;
                    }
                    else
                    {
                        int p = _context.SaleOrder.Select(s => s.Id).Max();
                        order.Id = p;
                        order.Id++;
                    }
                    order.CreatedDate = DateTime.Now;
                    order.Amount = totalCart();
                    _context.SaleOrder.Add(order);
                    _context.SaveChangesAsync();
                    List<ItemCart> carts = Carts;//lấy giỏ hàng
                    foreach (var item in carts)
                    {
                        OrderDetail d = new Models.OrderDetail();

                        if (_context.OrderDetail.Select(s => s.Id).Count() == 0)
                        {
                            d.Id = 1;
                        }
                        else
                        {
                            int p = _context.OrderDetail.Select(s => s.Id).Max();
                            d.Id = p;
                            d.Id++;
                        }
                        d.OrderId = order.Id;
                        d.ProductId = item.idCart;
                        d.Num = item.quantity.ToString();
                        d.UnitPrice = item.priceCart;


                        _context.OrderDetail.Add(d);
                    }
                    _context.SaveChangesAsync();
                    tranScope.Complete();
                    var data = HttpContext.Session.Get<List<ItemCart>>("gioHang");
                    data = null;
                    HttpContext.Session.Set("gioHang", data);
                }
                catch (Exception)
                {
                    tranScope.Dispose();
                    return RedirectToAction("CartIndex");

                }
            }
            return View();
        }

    }
}
