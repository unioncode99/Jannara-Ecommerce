import React from "react";
import AppSettings from "../AppSettings";
import { Menu } from "lucide-react";
import "./Navbar.css";

export default function Navbar({ onBurgerClick }) {
  return (
    <header className="navbar">
      <button className="burger-btn" onClick={onBurgerClick}>
        <Menu />
      </button>
      <div>
        <AppSettings />
      </div>
    </header>
  );
}
