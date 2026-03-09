import api from "./api";

export const getUsers = async (page, pageSize) => {
  const { data } = await api.get(`/users?page=${page}&pageSize=${pageSize}`);
  return data;
};

export const createUser = async (user) => {
  const response = await api.post("/users", user);
  return response.data;
};

export const deleteUser = async (id) => {
    const response = await api.delete(`/users/${id}`);
    return response.data;
}

export const changeRoleUser = async (id, role) => {
    const response = await api.patch(`/users/${id}/role`, {role});
    return response.data;
}

export const connectAddress = async (address) => {
  const response = await api.post("/users/wallet", {
  walletAddress: address});
    return response.data;
}

export const updateProfile = async (bio, tag) => {
  const response = await api.put("/users/profile", {bio, tag});

  return response.data;
}

export const updateNamespace = async (username) => {
  const response = await api.put("/users/username", {username});

  return response.data;
}