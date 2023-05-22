<template>
  <div class="home">
    <Modal class="modal is-active" v-show="isShowingSeeDetailsModal" @close="closeSeeDetails()">
      <template v-slot:header>{{seeDetailsDTo.customer.firstName}} {{seeDetailsDTo.customer.lastName}}</template>

      <template v-slot:body> 
        <div>
          <span>Amount Due: ${{amountOwed}}</span><br/>
          <span>Your Ticket Number Is: {{seeDetailsDTo.ticketNumber}}</span>
        </div>
      </template>
      <template v-slot:footer>Customer Phone Number: {{seeDetailsDTo.customer.phoneNumber}}</template>
    </Modal>
    <div class="landing-page" :style="backgroundStyle">
      <h1>Welcome valued employee!</h1>
      <h1 v-if="!canAddMore" class="full-lot-message">The lot is currently full!</h1>
    <h1 class="landing-title">{{ title }}</h1>
    <transaction-modal  class="newTransaction" v-on:updateParkingLot="getCarsInLot()" v-if="showingTransactionModal" ></transaction-modal>
    <div class="active-cars">
      <button v-if="canAddMore" v-on:click="displayTransactionModal()" class="logout-button">{{showingTransactionModal ? "Toggle Form" : "Add New Vehicle"}}</button>
      <div>{{ counter }}</div>
      <div v-for="car in activeCars" :key="car.licensePlate" class="car-container" > <!--@click="showPickupButton(car)"-->
        <div class="car-info">
          <p class="car-make">{{ car.car.make }}</p>
          <p class="car-model">{{ car.car.model }}</p>
          <p class="car-color">{{ car.car.color }}</p>
          <p class="car-license">{{ car.car.licensePlate }}</p>
          <div v-if="car.car.needsPickedUp">
              <h1 class="full-lot-message" >Come and get me in spot {{car.parkingSpotId}}</h1>
          </div>
          <button @click="showDetails(car.parkingSpotId)" class="pickup-button">See Details</button>
          <button @click="markAsPickedUp(car.car.licensePlate)" class="pickup-button">Pick Up</button>
        </div>
        <!--<button  @click="requestPickup(car)" class="pickup-button">Request Pickup</button> v-if="car.showPickupButton"-->
      </div>
    </div>
  </div>
  </div>
</template>

<script>
import TransactionModal from '../components/TransactionModal.vue'
import ParkingLotService from '../services/ParkingLotService'
import Modal from "../components/Modal"

export default {
  async mounted() {
    await this.getCarsInLot();
  },
  name: "home",
   data() {
    return {
      canAddMore: true,
      isShowingSeeDetailsModal: false,
      seeDetailsDTo: {},
      amountOwed: 0,
      showingTransactionModal: false,
      title: 'Active Cars',
      activeCars: [],
      gradientStartColor: '#7f0cf2',
      gradientEndColor: '#ff00dc',
      accentColor: '#ffc700',
      backgroundColor: '#4CBB17'
       };
  },
  methods: {
    closeSeeDetails(){
      this.isShowingSeeDetailsModal = false;
    },
    async showDetails(parkingSpotNumber){
      this.isShowingSeeDetailsModal = !this.isShowingSeeDetailsModal;
      const response = (await ParkingLotService.getParkingSpotDetails(parkingSpotNumber)).data;
      const dollarAmount = (await ParkingLotService.getTransactionAmount(response.ticketNumber)).data;
      this.seeDetailsDTo = response;
      this.amountOwed = dollarAmount;
    },
    async markAsPickedUp(licensePlate){
      console.log(licensePlate);
      console.log(this.$store.state.user.userId);
      if(confirm("Are you sure you want to remove this car from the parking lot?")){
        alert("This car will be removed from the parking lot.")
        await ParkingLotService.deleteCarsFromLot(licensePlate, this.$store.state.user.userId);
      }
      this.getCarsInLot();
    },
    displayTransactionModal(){
      if(this.canAddMore){
        this.showingTransactionModal = !this.showingTransactionModal;
      }
      else{
        this.showingTransactionModal = false;
      }
    },
    async getCarsInLot() {
      await ParkingLotService.getCarsParkedInLot().then((response) => {
        this.activeCars = response.data;
      });
      this.canAddMore = this.activeCars.length<10;
    }
  },
  components : {
    TransactionModal, Modal
  },
};
</script>

<style scoped>
h1 {
  font-size: 50px;
  color: white;
  text-align: center;
}
.landing-page {
  padding-top: 25px;
  display: flex;
  flex-direction: column;
  align-items: center;
  min-height: 100vh;
  width: 100%;
  color: #ffc700;
}

.landing-title {
  font-size: 2em;
  font-weight: bold;
  text-align: center;
  margin-bottom: 32px;
}

.active-cars {
  max-width: 800px;
  width: 90%;
}

.car-container {
  color: white;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px;
  margin-bottom: 16px;
  background-image: linear-gradient(to bottom right, #7f0cf2, #ff00dc);
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  cursor: pointer;
  transition: background-color 0.3s ease-in-out;
}

.car-container:hover {
  background-image: linear-gradient(to bottom right, #7f0cf2, dodgerblue);
}

.car-info {
  flex: 1;
}

.car-make {
  font-weight: bold;
}

.pickup-button {
  padding: 12px 16px;
  border: 1px solid black;
  border-radius: 4px;
  background-color: #ffc700;
  color: black;
  font-weight: normal;
  font-size: 15px;
  cursor: pointer;
  float: right;
}

.pickup-button:hover {
  background-image: linear-gradient(to bottom right, #7f0cf2, dodgerblue);
}

.logout-button:hover {
  background-image: linear-gradient(to bottom right, #7f0cf2, dodgerblue);
}

.logout-button {
  width: 100%;
  padding: 12px 16px;
  border: none;
  border-radius: 4px;
  background-color: #ffc700;
  color: black;
  font-weight: normal;
  font-size: 15px;
  margin-bottom: 15px;
}
.newTransaction{
  width: 100%;
}

.full-lot-message {
  font-size: 48px; /* Change the size to suit your needs */
  color: red;
  text-align: center;
  /* Add any additional styles you want for the warning sign */
  border: 2px solid red; /* Add border to the text */
  animation: pulsing 1s infinite; /* Add pulsing animation */
}

@keyframes pulsing {
  0% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.1);
  }
  100% {
    transform: scale(1);
  }
}
</style>

