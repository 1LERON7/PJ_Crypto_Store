import { ethers } from "ethers";
import ABI from "./abi.json";
import { keccak256, toUtf8Bytes } from "ethers";

const CONTRACT_ADDRESS = "0x923187efeDca0806de77A632e017B3bEeF8FC8A9";

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

const productHash = keccak256(toUtf8Bytes(productId));

  if (!productId) {
    throw new Error("productId is missing");
  }

  const contract = await getContract();

  const tx = await contract.buy(productHash, {
    value: ethers.parseEther(price.toString())
  });

  console.log("tx:", tx);

  await tx.wait();

  return tx.hash;
};