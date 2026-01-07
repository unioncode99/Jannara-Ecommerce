import UserCard from "./UserCard";
import "./CardView.css";

const CardView = ({ users, handleUserRoles }) => {
  return (
    <div className="card-view">
      {users?.map((user) => (
        <UserCard key={user.id} user={user} handleUserRoles={handleUserRoles} />
      ))}
    </div>
  );
};
export default CardView;
