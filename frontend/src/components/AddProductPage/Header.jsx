import { Link } from "react-router-dom";
import Button from "../ui/Button";
import "./Header.css";
import { ArrowLeft, ArrowRight, Save } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";

const Header = () => {
  const { translations, language } = useLanguage();
  const { title, cancel_text, save } = translations.general.pages.add_product;
  return (
    <header>
      <div>
        <Link to="/" className="back-home-link">
          {language == "en" ? <ArrowLeft /> : <ArrowRight />}
        </Link>
        <h2>{title}</h2>
      </div>
      <div>
        <Button className="cancel-btn">{cancel_text}</Button>
        <Button className="btn btn-primary save-btn">
          <Save />
          <span>{save}</span>
        </Button>
      </div>
    </header>
  );
};
export default Header;
