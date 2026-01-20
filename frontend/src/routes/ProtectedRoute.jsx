import { Navigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

export default function ProtectedRoute({ children, allowedRoles }) {
  const { user, token, currentRole } = useAuth();
  console.log("ProtectedRoute user:", user);
  console.log("ProtectedRoute token:", token);
  console.log("ProtectedRoute currentRole :", currentRole);

  // not logged in
  if (!user || !token) {
    return <Navigate to="/login" />;
  }

  // Role-based access control
  if (allowedRoles && !allowedRoles.includes(currentRole)) {
    return <Navigate to="/" replace />; // redirect if role not allowed
  }

  return children;
}
