import { Navigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

export default function ProtectedRoute({ children, allowedRoles }) {
  const { user, token } = useAuth();
  console.log("ProtectedRoute user:", user);
  console.log("ProtectedRoute token:", token);

  if (!user) {
    return <Navigate to="/login" />; // not logged in
  }

  // if (allowedRoles && !allowedRoles.includes(user.role)) {
  //   // role not allowed
  //   return <Navigate to="/login" replace />;
  // }

  return children;
}
