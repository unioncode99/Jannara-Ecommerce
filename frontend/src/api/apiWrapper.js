import axios from "axios";

const API_BASE_URL = "https://localhost:7179/api/";

const api = axios.create({
  baseURL: API_BASE_URL,
});

export const setAuthToken = (token) => {
  api.defaults.headers.common["Authorization"] = token
    ? `Bearer ${token}`
    : undefined;
};
// Interceptor: return only the response data
api.interceptors.response.use(
  (response) => response.data,
  (error) => {
    if (error.response && error.response.data) {
      return Promise.reject(error.response.data);
    }
    return Promise.reject(error);
  }
);

const apiWrapper = async (url, method = "GET", data = null) => {
  try {
    const config = { method, url, headers: {} };

    if (data instanceof FormData) {
      config.data = data;
    } else if (data) {
      config.headers["Content-Type"] = "application/json";
      config.data = data;
    }

    return await api(config);
  } catch (error) {
    console.log(error);
    // console.error("API error:", error);
    throw error;
  }
};

// CRUD
export const create = (url, data) => apiWrapper(url, "POST", data);
export const read = (url) => apiWrapper(url, "GET");
export const update = (url, data) => apiWrapper(url, "PUT", data);
export const remove = (url) => apiWrapper(url, "DELETE");

export default api;
