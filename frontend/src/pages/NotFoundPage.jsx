import { Link } from "react-router-dom";
import "./NotFoundPage.css";
import { useLanguage } from "../hooks/useLanguage";

const NotFoundPage = () => {
  const { translations } = useLanguage();
  return (
    <div className="not-found-page">
      <h1>404</h1>
      <h2>{translations.general.pages.Not_Found.title}</h2>
      <p>{translations.general.pages.Not_Found.subtitle}</p>
      {/* <Link to="/">{translations.general.pages.Not_Found.home_button}</Link> */}
      <Link to="/">{translations.general.pages.Not_Found.home_button}</Link>
    </div>
  );
};

export default NotFoundPage;
