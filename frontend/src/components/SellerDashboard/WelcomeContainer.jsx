import { useAuth } from "../../hooks/useAuth";
import { useLanguage } from "../../hooks/useLanguage";
import "./WelcomeContainer.css";

const WelcomeContainer = () => {
  const { translations } = useLanguage();
  const { person } = useAuth();

  const { welcome_title, welcome_description } =
    translations.general.pages.seller_dashboard;
  return (
    <div className="welcome-seller-container">
      <h1>
        <span>{welcome_title}</span>
        {person && person.firstName && person.lastName && (
          <span>, {person.firstName + " " + person.lastName}!</span>
        )}
      </h1>
      <p>{welcome_description}</p>
    </div>
  );
};
export default WelcomeContainer;
