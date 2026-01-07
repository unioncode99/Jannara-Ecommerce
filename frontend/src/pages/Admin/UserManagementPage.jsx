import { useEffect, useState } from "react";
import ViewSwitcher from "../../components/CustomerOrders/ViewSwitcher";
import AddEditProductCategoryModal from "../../components/ProductCategoriesManagementPage/AddEditProductCategoryModal";
import Button from "../../components/ui/Button";
import ConfirmModal from "../../components/ui/ConfirmModal";
import "./UserManagementPage.css";
import "../../hooks/useLanguage";
import { useLanguage } from "../../hooks/useLanguage";
import { Layers, Plus, User, UserPlus } from "lucide-react";
import CardView from "../../components/UserManagementPage.jsx/CardView";
import TableView from "../../components/UserManagementPage.jsx/TableView";
import AddAdminModal from "../../components/UserManagementPage.jsx/AddAdminModal";
import { read, update } from "../../api/apiWrapper";
import { toast } from "../../components/ui/Toast";
import UserRolesModal from "../../components/UserManagementPage.jsx/UserRolesModal";

const UserManagementPage = () => {
  const [view, setView] = useState("table"); // 'table' or 'card'
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isAddUserModalOpen, setIsAddUserModalOpen] = useState(false);
  const [isDactivateUserConfirmModalOpen, setIsDactivateUserConfirmModalOpen] =
    useState(false);
  const [selectedUser, setSelectedUser] = useState(null);
  const [selectedRole, setSelectedRole] = useState(null);
  const [isManageUserRolesModalOpen, setIsManageUserRolesModalOpen] =
    useState(false);
  const { translations } = useLanguage();
  const {
    title,
    add_admin,
    cancel,
    deactivate_role_success,
    activate_role_success,
    deactivate_role,
    activate_role,
    confirm_deactivate,
    confirm_activate,
    activate_role_error,
    deactivate_role_error,
  } = translations.general.pages.users_management;

  const fetchUsers = async () => {
    try {
      setLoading(true);
      const data = await read(`users?pageNumber=1&pageSize=10`); // for test
      setUsers(data?.items);
      console.log("data", data);
    } catch (error) {
      console.error("Failed to fetch categories:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  function handeAddAdmin() {}

  function handleToggleUserStatus(role) {
    console.log("role -> ", role);
    setSelectedRole(role);
    setIsDactivateUserConfirmModalOpen(true);
  }

  const closeModal = () => {
    setIsAddUserModalOpen(false);
    setIsDactivateUserConfirmModalOpen(false);
    setIsManageUserRolesModalOpen(false);
    setSelectedUser(null);
    setSelectedRole(null);
  };

  async function AddAdmin() {}

  async function ToggleUserStatus() {
    try {
      if (!selectedUser || !selectedUser.roles?.[0]) {
        return;
      }

      const updatedRole = {
        id: selectedUser.roles[0].id,
        isActive: !selectedUser.roles[0].isActive,
      };

      const data = await update(`user-roles/${selectedRole.id}`, updatedRole);

      if (selectedUser?.roles[0]?.isActive) {
        toast.show(deactivate_role_success, "success");
      } else {
        toast.show(activate_role_success, "success");
      }

      setUsers((prevUsers) =>
        prevUsers.map((u) =>
          u.id === selectedUser.id
            ? {
                ...u,
                roles: u.roles.map((r) =>
                  r.id === updatedRole.id
                    ? { ...r, isActive: updatedRole.isActive }
                    : r
                ),
              }
            : u
        )
      );
      console.log("data", data);
    } catch (error) {
      console.error("ToggleUserStatus:", error);
      if (selectedRole.isActive) {
        toast.show(deactivate_role_error, "success");
      } else {
        toast.show(activate_role_error, "success");
      }
    } finally {
      closeModal();
    }
  }

  function handleUserRoles(user) {
    setSelectedUser(user);
    setIsManageUserRolesModalOpen(true);
  }

  return (
    <div className="product-categori-management-container">
      <header>
        <h1>
          <User /> {title}
        </h1>
        <Button onClick={handeAddAdmin} className="btn btn-primary">
          <UserPlus /> {add_admin}
        </Button>
      </header>
      <ViewSwitcher view={view} setView={setView} />
      {view == "card" && (
        <CardView users={users} handleUserRoles={handleUserRoles} />
      )}
      {view == "table" && (
        <TableView users={users} handleUserRoles={handleUserRoles} />
      )}
      <AddAdminModal
        show={isAddUserModalOpen}
        onClose={closeModal}
        onConfirm={AddAdmin}
      />
      <UserRolesModal
        show={isManageUserRolesModalOpen}
        user={selectedUser}
        handleToggleUserStatus={handleToggleUserStatus}
        onClose={() => closeModal()}
      />
      <ConfirmModal
        show={isDactivateUserConfirmModalOpen}
        onClose={() => closeModal()}
        onConfirm={() => ToggleUserStatus()}
        title={selectedRole?.isActive ? confirm_deactivate : confirm_activate}
        cancelLabel={cancel}
        confirmLabel={selectedRole?.isActive ? deactivate_role : activate_role}
      />
    </div>
  );
};
export default UserManagementPage;
