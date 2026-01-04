import { createContext, useState } from "react";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [person, setPerson] = useState(null);
  const [token, setToken] = useState("");

  const login = (user, person, token) => {
    setUser(user);
    setPerson(person);
    setToken(token);
  };

  const logout = () => {
    setUser(null);
    setPerson(null);
    setToken("");
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
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};
