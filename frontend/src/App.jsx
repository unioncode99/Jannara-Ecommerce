import "./App.css";
import Register from "./pages/Auth/Register";
import { useLanguage } from "./hooks/useLanguage";
import LoginPage from "./pages/Auth/LoginPage";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import ToastContainer from "./components/ui/Toast";
import NotFoundPage from "./pages/NotFoundPage";
import AppSettings from "./components/AppSettings";
import ForgetPasswordPage from "./pages/Auth/ForgetPasswordPage";
import VerifyCodePage from "./pages/Auth/VerifyCodePage";
import ResetPasswordPage from "./pages/Auth/ResetPasswordPage";
import AccountConfirmationPage from "./pages/Auth/AccountConfirmationPage";
import ProtectedRoute from "./routes/ProtectedRoute";
import CustomerDashboard from "./pages/Customer/CustomerDashboard";
import SellerDashboard from "./pages/Seller/SellerDashboard";
import AdminDashboard from "./pages/Admin/AdminDashboard";
import DashboardLayout from "./components/MainLayout";
import MainLayout from "./components/MainLayout";
import Home from "./pages/Home";
import ProductPage from "./pages/ProductPage";
import WishListPage from "./pages/FavoritesPage";
import FavoritesPage from "./pages/FavoritesPage";
import CartPage from "./pages/CartPage";
import { CartProvider } from "./contexts/CartContext";
import CheckoutPage from "./pages/CheckoutPage";
import OrderSuccessPage from "./pages/OrderSuccessPage";
import CustomerOrders from "./pages/Customer/CustomerOrders";
import CustomerProfile from "./pages/Customer/CustomerProfile";
import ProductCategoriesManagementPage from "./pages/Admin/ProductCategoriesManagementPage";
import UserManagementPage from "./pages/Admin/UserManagementPage";
import AddProductPage from "./pages/Admin/AddProductPage";
import ProductsPage from "./pages/Admin/ProductsPage";
import BrandsPage from "./pages/Admin/BrandsPage";
import SellerOrders from "./pages/Seller/SellerOrders";
import SellerProducts from "./pages/Seller/SellerProducts";
import AddEditSellerProduct from "./pages/Seller/AddEditSellerProduct";

function App() {
  const { language } = useLanguage();

  return (
    // <div dir={language === "en" ? "ltr" : "rtl"}>
    <div className="container">
      <ToastContainer />
      <AppSettings isTopLeft={true} className="auth" />
      <BrowserRouter>
        <Routes>
          <Route element={<MainLayout />}>
            <Route path="/customer-dashboard" element={<CustomerDashboard />} />
            <Route path="/customer-orders" element={<CustomerOrders />} />
            <Route path="/seller-dashboard" element={<SellerDashboard />} />
            <Route path="/admin-dashboard" element={<AdminDashboard />} />
            <Route
              path="/product/:publicId"
              element={
                <CartProvider customerId={1}>
                  <ProductPage />
                </CartProvider>
              }
            />
            <Route path="/favorites" element={<FavoritesPage />} />
            <Route
              path="/cart"
              element={
                <CartProvider customerId={1}>
                  <CartPage />
                </CartProvider>
              }
            />
            <Route
              path="/checkout"
              element={
                <CartProvider customerId={1}>
                  <CheckoutPage />
                </CartProvider>
              }
            />
            <Route path="/" element={<Home />} />
            <Route path="/register" element={<Register />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/forget-password" element={<ForgetPasswordPage />} />
            <Route path="/verify-code" element={<VerifyCodePage />} />
            <Route path="/reset-password" element={<ResetPasswordPage />} />
            <Route
              path="/confirm-account"
              element={<AccountConfirmationPage />}
            />
            <Route
              path="order-success/:publicOrderId"
              element={<OrderSuccessPage />}
            />
            <Route path="customer-profile" element={<CustomerProfile />} />
            <Route
              path="product-categories"
              element={<ProductCategoriesManagementPage />}
            />
            <Route path="users" element={<UserManagementPage />} />
            <Route path="add-product" element={<AddProductPage />} />
            <Route
              path="edit-product/:productId"
              element={<AddProductPage />}
            />
            <Route path="products" element={<ProductsPage />} />
            <Route path="brands" element={<BrandsPage />} />
            <Route path="seller-orders" element={<SellerOrders />} />
            <Route path="seller-products" element={<SellerProducts />} />
            <Route
              path="add-seller-product"
              element={<AddEditSellerProduct />}
            />
            <Route
              path="edit-seller-product/:id"
              element={<AddEditSellerProduct />}
            />

            <Route path="*" element={<NotFoundPage />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
