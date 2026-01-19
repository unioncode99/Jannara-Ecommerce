import { useState } from "react";
import ReviewStars from "./ReviewStars";
import Button from "../ui/Button";
import TextArea from "../ui/TextArea";
import "./ReviewForm.css";
import { useLanguage } from "../../hooks/useLanguage";

const ReviewForm = ({ initialData, onSubmit, loading }) => {
  const [rating, setRating] = useState(initialData?.rating || 0);
  const [reviewText, setReviewText] = useState(initialData?.reviewText || "");

  const { translations } = useLanguage();
  const { write_review, submit_review, review_placeholder } =
    translations.general.reviews;

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!rating || rating > 5) {
      return;
    }

    onSubmit({
      rating,
      reviewText,
    });
  };

  return (
    <form onSubmit={handleSubmit} className="review-form">
      <h4>{write_review}</h4>

      <ReviewStars value={rating} onChange={setRating} />

      <TextArea
        value={reviewText}
        onChange={(e) => setReviewText(e.target.value)}
        placeholder={review_placeholder}
        rows="4"
      />

      <Button
        className="btn btn-primary"
        type="submit"
        disabled={loading || rating === 0}
      >
        {submit_review}
      </Button>
    </form>
  );
};

export default ReviewForm;
