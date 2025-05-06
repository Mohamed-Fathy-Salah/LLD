var displayBoard = new DisplayBoard();
var level1 = new Level(1, 
        [new CarSpot(1, 1), new MotorCycleSpot(1, 2), new TruckSpot(1, 3)],
        [displayBoard]);
var level2 = new Level(2, 
        [new CarSpot(1, 1), new MotorCycleSpot(1, 2), new TruckSpot(1, 3)],
        [displayBoard]);
var ParkingLot = new ParkingLot([level1, level2]);

var nearestAllocation = new NearestFirstAllocationStrategy();
var zonedAllocation = new ZonedAllocationStrategy();
var defaultAllocation = new DefaultAllocationStrategy();

var cashPayment = new CashPayment();

var car1 = new Car("1");
var ticket1 = ParkingLot.ParkVehicle(car1, defaultAllocation);
Thread.Sleep(1000);

var car2 = new Car("2");
var ticket2 = ParkingLot.ParkVehicle(car2, defaultAllocation);
Thread.Sleep(1000);

var car3 = new Car("3");
var ticket3 = ParkingLot.ParkVehicle(car3, defaultAllocation);
Thread.Sleep(1000);

var truck1 = new Truck("4");
var ticket4 = ParkingLot.ParkVehicle(truck1, defaultAllocation);
Thread.Sleep(1000);

var truck2 = new Truck("5");
var ticket5 = ParkingLot.ParkVehicle(truck2, defaultAllocation);
Thread.Sleep(1000);

ParkingLot.ExitVehicle(car1, cashPayment);
Thread.Sleep(1000);
ParkingLot.ExitVehicle(car1, cashPayment);
Thread.Sleep(1000);
ParkingLot.ExitVehicle(car2, cashPayment);
Thread.Sleep(1000);
ParkingLot.ExitVehicle(car3, cashPayment);
Thread.Sleep(1000);
ParkingLot.ExitVehicle(truck1, cashPayment);
Thread.Sleep(1000);
ParkingLot.ExitVehicle(truck2, cashPayment);
Thread.Sleep(1000);
