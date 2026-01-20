import { createContext, useEffect, useState } from "react";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [person, setPerson] = useState(null);
  const [token, setToken] = useState("");
  const [currentRole, setCurrentRole] = useState(() => {
    const savedRole = localStorage.getItem("currentRole");
    return savedRole || "unknown_user";
  });

  useEffect(() => {
    if (user?.roles?.length) {
      const defaultRole = user.roles[0].nameEn.toLowerCase();
      setCurrentRole(defaultRole);
      localStorage.setItem("currentRole", defaultRole);
    }
  }, [user]);

  const changeRole = (role) => {
    setCurrentRole(role);
    localStorage.setItem("currentRole", role);
  };

  const login = (user, person, token) => {
    setUser(user);
    setPerson(person);
    setToken(token);
  };

  const logout = () => {
    setUser(null);
    setPerson(null);
    setToken("");
    setCurrentRole("unknown_user");
    localStorage.removeItem("currentRole");
  };
  const refreshUser = (updatedUser) => {
    setUser(updatedUser);
  };
  return (
    <AuthContext.Provider
      value={{
        user,
        setUser,
        person,
        setPerson,
        token,
        login,
        logout,
        refreshUser,
        currentRole,
        changeRole,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};
