const API_BASE_URL = "https://localhost:7094/api/";

const apiWrapper = async (
  url,
  method = "GET",
  body = null,
  language = "en"
) => {
  const options = { method, headers: {} };

  if (body) {
    if (body instanceof FormData) {
      // ✅ Do NOT set Content-Type for FormData
      options.body = body;
    } else {
      // JSON body
      options.headers["Content-Type"] = "application/json";
      options.headers["Accept-Language"] = language;
      options.body = JSON.stringify(body);
    }
  }

  try {
    const response = await fetch(`${API_BASE_URL}${url}`, options);

    if (!response.ok) {
      const errorData = await response.json();
      // If response is not OK (status code 4xx, 5xx)
      throw new Error(errorData.message);
    }

    // Check if the response is JSON, otherwise return the raw response
    try {
      return await response.json();
    } catch (error) {
      console.log(error);
      return {}; // If there's no JSON body (like for DELETE requests), return an empty object
    }
  } catch (error) {
    console.log(error);
    console.error("API error:", error);
    throw error; // Propagate the error
  }
};

// CRUD functions
export const create = (url, data, language) =>
  apiWrapper(url, "POST", data, language);
export const read = (url, language) => apiWrapper(url, "GET", null, language);
export const update = (url, data, language) =>
  apiWrapper(url, "PUT", data, language);
export const remove = (url, language) =>
  apiWrapper(url, "DELETE", null, language); // No body required for DELETE requests
