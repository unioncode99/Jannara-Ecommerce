import UserCard from "./UserCard";
import "./CardView.css";

const CardView = ({ users, handleDactivateUser }) => {
  return (
    <div className="card-view">
      {users?.map((user) => (
        <UserCard
          key={user.id}
          user={user}
          handleDactivateUser={handleDactivateUser}
        />
      ))}
    </div>
  );
};
export default CardView;
