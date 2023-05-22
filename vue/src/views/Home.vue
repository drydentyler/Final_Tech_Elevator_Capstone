<template>
  <div class="home">
<div class="landing-page">
  <h1>Welcome!</h1>
    <h2>Enter your ticket number to get your balance and total time:</h2>  
  <div class="active-cars">
    <h3>Enter Ticket Number Here:</h3>
   <input required v-model="ticketNumber"  type="text" />
   <button type="button" class="btn" @click="openModal()">See Details</button>
    <br/>
    <button @click="togglePickUp(); showGame = true" type="button" class="btn" >Submit</button>
    <Modal class="modal" v-show="seeDetailsVisible" @close="close">
      <template v-slot:header> {{customerInfo.firstName}} {{customerInfo.lastName}}</template>

      <template v-slot:body> Your vehicle has been with us for: {{timePassed}}</template>

      <template v-slot:footer> Your total balance is: ${{ticketAmount}}</template>
    </Modal>
    <Modal class="modal" v-show="pickUpRequestVisible" @close="close">
      <template v-slot:header> {{customerInfo.firstName}} {{customerInfo.lastName}} </template>

      <template v-slot:body>Your car is being picked up!</template>

      <template v-slot:footer> Your total balance is: ${{ticketAmount}}</template>
    </Modal>
</div>  
    <the-game v-if="showGame"></the-game>
  </div>
  </div>
</template>

<script>

import Modal from "../components/Modal"
import ParkingLotService from '../services/ParkingLotService'
import TheGame from '../components/TheGame.vue';

export default 
  {
  name: "home",
   components: {
     Modal,
     TheGame
  },
  data() {
    return {
      customerInfo: {

      },
      seeDetailsVisible: false,
      pickUpRequestVisible: false,
      ticketNumber: "",
      ticketAmount: 0,
      timePassed: "",
      showGame: false
    }
  },
  methods: {
    togglePickUp(){
      ParkingLotService.getTransactionAmount(this.ticketNumber).then((response) => {
        this.ticketAmount = response.data;
        });
      this.pickUpRequestVisible = !this.pickUpRequestVisible;
      ParkingLotService.toggleNeedsPickedUp(this.ticketNumber);
      console.log("working");
    },
    async openModal() {
      ParkingLotService.getTransactionAmount(this.ticketNumber).then((response) => {
        this.ticketAmount = response.data;
        });
      ParkingLotService.getTimeParked(this.ticketNumber).then((response)=> {
        this.timePassed = response.data;
      });
      const customerResponse = (await ParkingLotService.getCustomerByTicket(this.ticketNumber)).data;
      this.customerInfo = customerResponse;
      this.seeDetailsVisible = true;
    },
    close() {
      this.seeDetailsVisible = false;
      this.pickUpRequestVisible = false;
    },
    increaseCounter() {
      this.counter++;
    }

  }
};
</script>

<style scoped>
h1 {
  font-size: 72px;
}
h2 {
  font-size: 25px;
  padding-bottom: 50px;
}
.landing-page {
  padding-top: 25px;
  display: flex;
  flex-direction: column;
  align-items: center;
  min-height: 100vh;
  width: 100%;
  color: white;
}

.landing-title {
  font-size: 2em;
  font-weight: bold;
  text-align: center;
  margin-bottom: 32px;
}

.active-cars {
  max-width: 800px;
  width: 15%;
  display: flex;
  flex-direction: column;
  align-items: center;
  color: white;
  background: rgba(255, 255, 255, 0.1); 
  border-radius: 10px; 
  backdrop-filter: blur(3px); 
  border: 1px solid rgba(255, 255, 255, 0.2);
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  padding: 16px;
  margin-bottom: 16px;
 
}

.car-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px;
  margin-bottom: 16px;
  background-color: #4CBB17;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  cursor: pointer;
  transition: background-color 0.3s ease-in-out;
}

.car-container:hover {
  background-color:magenta;
}

.car-info {
  flex: 1;
}

.car-make {
  font-weight: bold;
}

.pickup-button {
  padding: 12px 16px;
  border: none;
  border-radius: 4px;
  background-color: #ffc700;
  color: white;
  font-weight: bold;
  cursor: pointer;
}

.btn {
  width: 50%;
  padding: 12px 16px;
  border: none;
  border-radius: 4px;
  background-color: #7f0cf2;
  color: white;
}
.btn:hover {
  background-image: linear-gradient(to bottom right, #7f0cf2, dodgerblue);
}

input {
  margin: 5px 10px 10px 10px;
}


@media screen and (max-width: 768px) {
  .active-cars {
    width: 70%;
  }
}
</style>

