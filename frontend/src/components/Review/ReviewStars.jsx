import { Star } from "lucide-react";
import "./ReviewStars.css";

const ReviewStars = ({ value = 0, onChange, readOnly = false }) => {
  return (
    <div className="review-stars">
      {[1, 2, 3, 4, 5].map((star) => (
        <Star
          key={star}
          size={20}
          className={`star ${star <= value ? "filled" : ""}`}
          onClick={() => !readOnly && onChange(star)}
        />
      ))}
    </div>
  );
};

export default ReviewStars;
