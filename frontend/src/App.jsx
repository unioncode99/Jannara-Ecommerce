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
import Profile from "./pages/Customer/Profile";
import ProductCategoriesManagementPage from "./pages/Admin/ProductCategoriesManagementPage";
import UserManagementPage from "./pages/Admin/UserManagementPage";
import AddProductPage from "./pages/Admin/AddProductPage";
import ProductsPage from "./pages/Admin/ProductsPage";
import BrandsPage from "./pages/Admin/BrandsPage";
import SellerOrders from "./pages/Seller/SellerOrders";
import SellerProducts from "./pages/Seller/SellerProducts";
import AddEditSellerProduct from "./pages/Seller/AddEditSellerProduct";
import UnauthorizedPage from "./pages/UnauthorizedPage";

function App() {
  const { language } = useLanguage();

  return (
    // <div dir={language === "en" ? "ltr" : "rtl"}>
    <div className="container">
      <ToastContainer />
      <AppSettings isTopLeft={true} className="auth" />
      <BrowserRouter>
        <Routes>
          {/* Layout wrapper */}
          <Route element={<MainLayout />}>
            {/* Public routes */}
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
              path="/product/:publicId"
              element={
                <CartProvider>
                  <ProductPage />
                </CartProvider>
              }
            />
            <Route path="/favorites" element={<FavoritesPage />} />
            <Route
              path="/cart"
              element={
                <CartProvider>
                  <CartPage />
                </CartProvider>
              }
            />

            <Route
              path="order-success/:publicOrderId"
              element={<OrderSuccessPage />}
            />

            {/* Unauthorized page */}
            <Route path="unauthorized" element={<UnauthorizedPage />} />

            {/* Protected routes */}
            <Route
              path="profile"
              element={
                <ProtectedRoute
                  allowedRoles={["customer", "seller", "admin", "superadmin"]}
                >
                  <Profile />
                </ProtectedRoute>
              }
            />
            {/* Customer-only routes */}
            <Route element={<ProtectedRoute allowedRoles={["customer"]} />}>
              <Route
                path="/customer-dashboard"
                element={<CustomerDashboard />}
              />
              <Route path="/customer-orders" element={<CustomerOrders />} />
              <Route
                path="/checkout"
                element={
                  <CartProvider>
                    <CheckoutPage />
                  </CartProvider>
                }
              />
            </Route>

            {/* Seller-only routes */}
            <Route element={<ProtectedRoute allowedRoles={["seller"]} />}>
              <Route path="/seller-dashboard" element={<SellerDashboard />} />
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
            </Route>

            {/* Admin-only routes */}
            <Route element={<ProtectedRoute allowedRoles={["admin"]} />}>
              <Route path="/admin-dashboard" element={<AdminDashboard />} />
              <Route path="users" element={<UserManagementPage />} />
              <Route
                path="product-categories"
                element={<ProductCategoriesManagementPage />}
              />
              <Route path="add-product" element={<AddProductPage />} />
              <Route
                path="edit-product/:productId"
                element={<AddProductPage />}
              />
              <Route path="products" element={<ProductsPage />} />
              <Route path="brands" element={<BrandsPage />} />
            </Route>
            {/* Catch-all 404 */}
            <Route path="*" element={<NotFoundPage />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
