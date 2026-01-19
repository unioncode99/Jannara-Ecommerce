import { Star } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import "./ReviewCard.css";

const ReviewCard = ({ review }) => {
  let { customerName, rating, reviewText, createdAt, profileImage } = review;

  const formattedDate = createdAt
    ? new Date(createdAt).toLocaleDateString()
    : "";

  const { translations } = useLanguage();
  const { reviewed_on, anonymous } = translations.general.reviews;

  return (
    <li className="review-card">
      <div className="review-card-header">
        {profileImage ? (
          <img
            src={profileImage}
            alt={customerName || "Anonymous"}
            className="reviewer-image"
          />
        ) : (
          <div className="reviewer-placeholder">
            {(customerName || anonymous)[0]}
          </div>
        )}
        <div className="reviewer-info">
          <strong>{customerName || anonymous}</strong>
          {formattedDate && (
            <small className="review-date">
              {reviewed_on}: {formattedDate}
            </small>
          )}
        </div>
        <span className="review-rating">
          ({rating} <Star />)
        </span>
      </div>
      <p className="review-text">{reviewText}</p>
    </li>
  );
};

export default ReviewCard;
