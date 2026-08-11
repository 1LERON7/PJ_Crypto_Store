export const connectWallet = async () => {
  if (!window.ethereum) {
    alert("Install MetaMask");
    return null;
  }

  const accounts = await window.ethereum.request({
    method: "eth_requestAccounts"
  });

  return accounts[0];
};