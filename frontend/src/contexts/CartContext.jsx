import { createContext, useContext, useEffect, useState } from "react";
import { create, read, remove } from "../api/apiWrapper";

const CartContext = createContext(null);

export const CartProvider = ({ customerId, children }) => {
  const [cart, setCart] = useState(null);
  const [loading, setLoading] = useState(false);
  const [itemLoading, setItemLoading] = useState({}); // per-item loading
  const [error, setError] = useState(null);

  const fetchCart = async () => {
    try {
      setLoading(true);
      const data = await read(`cart?customerId=${customerId}`);
      console.log("data -> ", data);
      setCart(data);
    } catch (err) {
      setError("Failed to load cart", err);
    } finally {
      setLoading(false);
    }
  };

  const addOrUpdateItem = async (payload) => {
    try {
      setItemLoading(true);
      await create("cart", payload);
      await fetchCart();
    } catch (err) {
      setError("Failed to update cart");
      throw err;
    } finally {
      setItemLoading(false);
    }
  };

  const removeItem = async (cartItemId) => {
    try {
      setItemLoading(true);
      await remove(`cart/item/${cartItemId}`);
      await fetchCart(); // 🔁 refresh cart
    } catch (err) {
      setError("Failed to remove item");
      throw err;
    } finally {
      setItemLoading(false);
    }
  };

  const clearCart = async (cartId) => {
    try {
      setLoading(true);
      await create(`cart/clear/${cartId}`);
      await fetchCart(); // 🔁 refresh cart
    } catch (err) {
      setError("Failed to update cart");
      throw err;
    } finally {
      setLoading(false);
    }
  };

  const isInCart = (sellerProductId) => {
    if (!cart?.cartItems) return false;
    return cart.cartItems.some(
      (item) => item.sellerProductId === sellerProductId
    );
  };

  /* =========================
     Initial Load
  ========================== */
  useEffect(() => {
    if (customerId) {
      fetchCart();
    }
  }, [customerId]);
  //   useEffect(() => {
  //     fetchCart();
  //   }, []);

  return (
    <CartContext.Provider
      value={{
        cart,
        loading,
        error,
        fetchCart,
        addOrUpdateItem,
        removeItem,
        clearCart,
        isInCart,
      }}
    >
      {children}
    </CartContext.Provider>
  );
};

/* =========================
   Hook
========================== */
export const useCart = () => {
  const ctx = useContext(CartContext);
  if (!ctx) {
    throw new Error("useCart must be used inside CartProvider");
  }
  return ctx;
};
