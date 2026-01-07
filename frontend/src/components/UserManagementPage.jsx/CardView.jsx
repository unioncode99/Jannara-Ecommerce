import UserCard from "./UserCard";
import "./CardView.css";

const CardView = ({ users, handleToggleUserStatus }) => {
  return (
    <div className="card-view">
      {users?.map((user) => (
        <UserCard
          key={user.id}
          user={user}
          handleToggleUserStatus={handleToggleUserStatus}
        />
      ))}
    </div>
  );
};
export default CardView;
