import { ethers } from "ethers";
import axios from "axios";

export async function pay(paymentId, amountEth, orderId) {

    if (!window.ethereum) {
        alert("MetaMask not installed");
        return;
    }

    const provider = new ethers.BrowserProvider(window.ethereum);
    const signer = await provider.getSigner();

    const contractAddress = "0xYOUR_CONTRACT_ADDRESS";          // TODOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
    const abi = [ "function pay(uint orderId) public payable" ];// TODOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO

    const contract = new ethers.Contract(contractAddress, abi, signer);

    const tx = await contract.pay(orderId, {
        value: ethers.parseEther(amountEth.toString())
    });

    const receipt = await tx.wait(); // ждем подтверждение

    // отправляем txhash в бек
    await axios.post("/api/payments/confirm", {
        paymentId: paymentId,
        txHash: tx.hash
    });

    alert("Payment sent!");
}