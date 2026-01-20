import { Link } from "react-router-dom";
import { useLanguage } from "../hooks/useLanguage";
import "./UnauthorizedPage.css";

const UnauthorizedPage = () => {
  const { translations } = useLanguage();

  const { title, subtitle, home_button } =
    translations.general.pages.unauthorized;

  return (
    <div className="not-found-page">
      <h1>403</h1>
      <h2>{title}</h2>
      <p>{subtitle}</p>
      <Link to="/">{home_button}</Link>
    </div>
  );
};
export default UnauthorizedPage;
