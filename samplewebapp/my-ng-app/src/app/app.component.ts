import { Component, OnInit } from "@angular/core";
import { SwPush } from "@angular/service-worker";
import { HttpClient } from "@angular/common/http";
import { PushNotification } from "./model";

@Component({
  selector: "app-root",
  templateUrl: "app.component.html",
  styles: [],
})
export class AppComponent implements OnInit {
  title = "my-ng-app";
  message = "";
  result = "";

  constructor(private swPush: SwPush, private http: HttpClient) {}

  ngOnInit(): void {
    this.swPush.messages.subscribe((data) => {
      this.result = JSON.stringify(data);
    });
  }

  async subscribePushNotifications(): Promise<void> {
    if (!this.swPush.isEnabled) {
      alert("Your browser is too old to support this feature.");
      return;
    }
    const response = await this.http
      .get<{ publicKey: string }>("http://localhost:13884/vapidpublickey")
      .toPromise();
    try {
      const sub = await this.swPush.requestSubscription({
        serverPublicKey: response.publicKey,
      });
      await this.http
        .post("http://localhost:13884/subscribe/arunktha", {
          Subscription: sub,
        })
        .toPromise();
    } catch (err) {
      console.error("Could not subscribe to notifications", err);
    }
  }

  async sendNotification() {
    this.result = "";
    await this.http
      .post("http://localhost:13884/send/arunktha", <PushNotification>{
        title: "Good day!!!",
        body: this.message,
        icon: "/assets/icons/icon-144x144.png",
        actions: [
          { action: "/do/something/", title: "Do something" },
          { action: "/do/something/", title: "Do something again" },
        ],
      })
      .toPromise();
    this.message = "";
  }

  async unsubscribePushNotifications() {
    await this.http
      .delete("http://localhost:13884/unsubscribe/arunktha")
      .toPromise();
    this.swPush.unsubscribe();
  }
}
