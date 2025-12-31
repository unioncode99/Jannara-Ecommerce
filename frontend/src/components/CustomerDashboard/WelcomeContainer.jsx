import { Link } from "react-router-dom";
import "./WelcomeContainer.css";
import { useLanguage } from "../../hooks/useLanguage";
import { useAuth } from "../../hooks/useAuth";
import { ArrowLeft, ArrowRight } from "lucide-react";

const WelcomeContainer = () => {
  const { language, translations } = useLanguage();
  const { person } = useAuth();
  const { welcome_title, welcome_description, shop_now } =
    translations.general.pages.customer_dashboard;

  return (
    <div className="welcome-customer-container">
      <h1>
        <span>{welcome_title}</span>
        {person && person.firstName && person.lastName && (
          <span>, {person.firstName + " " + person.lastName}!</span>
        )}
      </h1>
      <p>{welcome_description}</p>
      <Link to="/" className="btn btn-primary show-now-btn">
        {shop_now}
        {language == "en" ? <ArrowRight /> : <ArrowLeft />}
      </Link>
    </div>
  );
};
export default WelcomeContainer;
