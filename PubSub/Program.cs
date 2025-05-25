var pubsub = new PubSubSystem();

var subscriber1 = new Subscriber();
var subscriber2 = new Subscriber();

var topic1 = "topic1";
var topic2 = "topic2";

pubsub.subscribe(topic1, subscriber1);
pubsub.subscribe(topic1, subscriber2);
pubsub.subscribe(topic2, subscriber1);
pubsub.subscribe(topic2, subscriber1);
pubsub.subscribe(topic2, subscriber1);

pubsub.publish(topic1, "message 1");
pubsub.publish(topic1, "message 2");
pubsub.publish(topic2, "message 3");
