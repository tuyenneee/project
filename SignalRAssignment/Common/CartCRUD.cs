using SignalRAssignment.Models;

namespace SignalRAssignment.Common
{
    

    public class Cart
    {

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        
        public decimal Total => CartItems.Sum(c => c.Total);

        public int Count => CartItems.Count();
    }

    public class CartItem
    {
        public Product Product { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Total => Product.UnitPrice * Quantity;
    }
    /// <summary>
    /// This class is used to perform CRUD operations on the cart in the session.
    /// </summary>
    public class CartCRUD
    {
        private readonly PizzaStoreContext _context;
        private readonly ISession _session;

        public CartCRUD(PizzaStoreContext context, ISession session)
        {
            _context = context;
            _session = session;
        }

        /// <summary>
        /// This method is used to add a product to the cart.
        /// </summary>
        /// <param name="productId">The id of the product to add to the cart.</param>
        /// <param name="quantity">The quantity of the product to add to the cart.</param>
        public void AddToCart(int productId, int quantity)
        {
            
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return;
            }
            var cart = VaSession.Get<Cart>(_session, "Cart") ?? new Cart();
            var cartItem = cart.CartItems.FirstOrDefault(c => c.Product.ProductId == productId);
            if (cartItem == null)
            {
                cart.CartItems.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                cartItem.Quantity += quantity;
            }
            VaSession.Set(_session, "Cart", cart);
        }

        /// <summary>
        /// This method is used to remove a product from the cart.
        /// </summary>
        /// <param name="productId">The id of the product to remove from the cart.</param>
        public void RemoveFromCart(int productId)
        {
            try
            {
                var cart = VaSession.Get<Cart>(_session, "Cart");
                if (cart == null)
                {
                    return;
                }
                var cartItem = cart.CartItems.FirstOrDefault(c => c.Product.ProductId == productId);
                if (cartItem == null)
                {
                    return;
                }
                cart.CartItems.Remove(cartItem);
                VaSession.Set(_session, "Cart", cart);
            }catch(Exception ex)
            {

            }
        }

        /// <summary>
        /// This method is used to update the quantity of a product in the cart.
        /// </summary>
        /// <param name="productId">The id of the product to update the quantity of.</param>
        /// <param name="quantity">The new quantity of the product.</param>
        public void UpdateQuantity(int productId, bool isAdd=false, bool isRemove=false, int quantity = 0)
        {
            var cart = VaSession.Get<Cart>(_session, "Cart");
            if (cart == null)
            {
                return;
            }
            var cartItem = cart.CartItems.FirstOrDefault(c => c.Product.ProductId == productId);
            if (cartItem == null)
            {
                return;
            }
            if(isAdd){
                cartItem.Quantity = cartItem.Quantity + 1;
            }
            if(isRemove){
                if(cartItem.Quantity>1)
                cartItem.Quantity = cartItem.Quantity - 1;
               
            }
            if(!isAdd && !isRemove){
                cartItem.Quantity = quantity;
            }
            VaSession.Set(_session, "Cart", cart);
        }

        /// <summary>
        /// This method is used to clear the cart.
        /// </summary>

        public void ClearCart()
        {
            _session.Remove("Cart");
        }

        /// <summary>
        /// This method is used to get the total of the cart.
        /// </summary>
        /// <returns>The total of the cart.</returns>
        public decimal GetTotalQty()
        {
            var cart = VaSession.Get<Cart>(_session, "Cart");
            if (cart == null)
            {
                return 0;
            }
            return cart.Count;
        }

        /// <summary>
        /// This method is used to get the total of the cart.
        /// </summary>
        /// <returns>The total of the cart.</returns>
        public decimal GetTotal()
        {
            var cart = VaSession.Get<Cart>(_session, "Cart");
            if (cart == null)
            {
                return 0;
            }
            return cart.Total;
        }

        /// <summary>
        /// This method is used to get the cart.
        /// </summary>
        /// <returns>The cart.</returns>
        public Cart GetCart()
        {
            return VaSession.Get<Cart>(_session, "Cart") ?? new Cart();
        }

        /// <summary>
        /// This method is used to get the cart items.
        /// </summary>
        /// <returns>The cart items.</returns>
        public List<CartItem> GetCartItems()
        {
            var cart = VaSession.Get<Cart>(_session, "Cart");
            if (cart == null)
            {
                return new List<CartItem>();
            }
            return cart.CartItems;
        }
        
    }
}
