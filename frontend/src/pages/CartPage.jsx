import { Link } from "react-router-dom";
import CartItemCard from "../components/CartPage/CartItemCard";
import { useCart } from "../contexts/CartContext";
import { ArrowLeft, ArrowRight, ShoppingBag } from "lucide-react";
import "./CartPage.css";
import { formatMoney } from "../utils/utils";
import { useLanguage } from "../hooks/useLanguage";

const CartPage = () => {
  const { cart, clearCart } = useCart();
  const { translations, language } = useLanguage();

  const {
    title,
    items,
    clear_cart,
    order_summary,
    subtotal,
    shipping,
    tax,
    total,
    free,
    checkout,
    empty_title,
    empty_desc,
    start_shopping,
  } = translations.general.pages.cart;

  return (
    <>
      {cart?.cartItems?.length > 0 ? (
        <div>
          <div className="cart-header-container">
            <h1>{title}</h1>
            <div>
              <span>
                {cart?.itemsCount} {items}
              </span>
              <button
                className="clear-cart-btn"
                onClick={() => clearCart(cart.id)}
              >
                {clear_cart}
              </button>
            </div>
          </div>
          <div className="cart-content-container">
            <div className="cart-items-container">
              {cart?.cartItems?.map((item) => (
                <CartItemCard key={item.id} cartItem={item} />
              ))}
            </div>
            <div className="order-summary-container">
              <h2>{order_summary}</h2>
              <div>
                <div>
                  <span>{subtotal}</span>
                  {cart.subTotal === 0 ? (
                    <span className="free">{free}</span>
                  ) : (
                    <span>{formatMoney(cart.subTotal)}</span>
                  )}
                </div>
                <div>
                  <span>{shipping}</span>
                  {cart.shippingPrice === 0 ? (
                    <span className="free">{free}</span>
                  ) : (
                    <span>{formatMoney(cart.shippingPrice)}</span>
                  )}
                </div>
                <div>
                  <span>{tax}</span>

                  {cart.taxPrice === 0 ? (
                    <span className="free">{free}</span>
                  ) : (
                    <span>{formatMoney(cart.taxPrice)}</span>
                  )}
                </div>
                <div>
                  <span>{total}</span>
                  {cart.grandTotal === 0 ? (
                    <span className="free">{free}</span>
                  ) : (
                    <span>{formatMoney(cart.grandTotal)}</span>
                  )}
                </div>
              </div>
              <Link
                to="/checkout"
                className="btn btn-primary btn-block checkout-btn"
              >
                {checkout}
                {language == "en" ? <ArrowRight /> : <ArrowLeft />}
              </Link>
            </div>
          </div>
        </div>
      ) : (
        <div className="empty-cart">
          <div>
            <ShoppingBag className="shopping-bag-svg" />
            <h1>{empty_title}</h1>
            <p>{empty_desc}</p>
            <Link
              to="/"
              className="btn btn-primary btn-block start-shopping-btn"
            >
              {start_shopping}
              {language == "en" ? <ArrowRight /> : <ArrowLeft />}
            </Link>
          </div>
        </div>
      )}
    </>
  );
};
export default CartPage;
