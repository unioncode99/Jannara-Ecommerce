import { Link, useNavigate } from "react-router-dom";
import Button from "../ui/Button";
import "./Header.css";
import { ArrowLeft, ArrowRight, Save } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";

const Header = ({ isModeUpdate }) => {
  const { translations, language } = useLanguage();
  const { title, cancel_text, save } = translations.general.pages.add_product;
  const { edit_product } = translations.general.pages.products;
  const navigate = useNavigate();

  return (
    <header>
      <div>
        <Link to="/products" className="back-home-link">
          {language == "en" ? <ArrowLeft /> : <ArrowRight />}
        </Link>
        <h2>{isModeUpdate ? edit_product : title}</h2>
      </div>
      <div>
        <Button onClick={() => navigate(`/products`)} className="cancel-btn">
          {cancel_text}
        </Button>
        {!isModeUpdate && (
          <Button className="btn btn-primary save-btn">
            <Save />
            <span>{save}</span>
          </Button>
        )}
      </div>
    </header>
  );
};
export default Header;
