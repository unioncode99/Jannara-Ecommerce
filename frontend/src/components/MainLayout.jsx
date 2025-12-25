import { Outlet } from "react-router-dom";
import Sidebar from "./ui/Sidebar";
import Navbar from "./ui/Navbar";
import "./MainLayout.css";
import { useAuth } from "../hooks/useAuth";
import { useState } from "react";

export default function MainLayout() {
  const [isSibebarOpen, setIsSibebarOpen] = useState(false);
  const { user } = useAuth();

  const openSidebar = function () {
    setIsSibebarOpen(true);
    console.log(user);
  };

  const closeSidebar = function () {
    setIsSibebarOpen(false);
  };

  return (
    <div>
      <Navbar onBurgerClick={openSidebar} />
      <Sidebar isSibebarOpen={isSibebarOpen} onClose={closeSidebar} />
      <main className="main">
        <div style={{ padding: "20px" }}>
          <Outlet /> {/* Nested pages */}
        </div>
      </main>
    </div>
  );
}
