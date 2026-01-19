import ReviewCard from "./ReviewCard";

const ReviewList = ({ reviews }) => {
  if (!reviews || reviews.length === 0) return <p>No reviews yet.</p>;

  return (
    <ul className="product-reviews-list">
      {reviews.map((review) => (
        <ReviewCard key={review.id} review={review} />
      ))}
    </ul>
  );
};

export default ReviewList;
