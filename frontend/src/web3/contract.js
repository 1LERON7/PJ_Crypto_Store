import { ethers } from "ethers";
import ABI from "./abi.json";
import { keccak256, toUtf8Bytes } from "ethers";
import { confirmPayment } from "../api/payments";

const CONTRACT_ADDRESS = "0xBD085265028885Ec8610B89ed3900aD5880Ffab9";

export const getContract = async () => {

  if (!window.ethereum) {
    throw new Error("MetaMask not installed");
  }

  const provider = new ethers.BrowserProvider(window.ethereum);

  await provider.send("eth_requestAccounts", []);

  const signer = await provider.getSigner();

  const contract = new ethers.Contract(
    CONTRACT_ADDRESS,
    ABI,
    signer
  );

  return contract;
};

export const buyProduct = async (productId, price) => {

  console.log("productId:", productId)
const productHash = keccak256(toUtf8Bytes(productId));
console.log("productHash:", productHash)

  if (!productId) {
    throw new Error("productId is missing");
  }

  const contract = await getContract();

  const tx = await contract.buy(productHash, {
    value: ethers.parseEther(price.toString())
  });

  console.log("tx:", tx);

  await tx.wait();

  const txHash = tx.hash;

  await confirmPayment(productId, txHash);

  return tx.hash;
};