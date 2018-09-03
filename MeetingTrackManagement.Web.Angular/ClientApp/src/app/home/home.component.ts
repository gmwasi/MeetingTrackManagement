import { Component, Inject, OnDestroy, } from '@angular/core';
import {FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs/Subscription';
import { MeetingService } from '../meeting-service.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent {
  public meetingForm: FormGroup;
  public output: string;
  private _url = './api/Meeting';
  public getMeeting$: Subscription;
  private _meetingService: MeetingService;

  constructor(@Inject('BASE_URL') baseUrl: string, private fb: FormBuilder, private meetingService: MeetingService) {
    this.meetingForm = this.fb.group({
      // tslint:disable-next-line:max-line-length
      input: ['Writing Fast Tests Against Enterprise Rails: 60min\r\nOverdoing it in Python: 45min\r\nLua for the Masses: 30min\r\nRuby Errors from Mismatched Gem Versions: 45min\r\nCommon Ruby Errors: 45min\r\nRails for Python Developers lightning: 35min\r\nCommunicating Over Distance: 60min\r\nAccounting-Driven Development: 45min\r\nWoah: 30min\r\nSit Down and Write: 30min\r\nPair Programming vs Noise: 45min\r\nRails Magic: 60min\r\nRuby on Rails: Why We Should Move On?: 60min\r\nClojure Ate Scala (on my project): 45min\r\nProgramming in the Boondocks of Seattle: 30min\r\nRuby vs. Clojure for Back-End Development: 30min\r\nRuby on Rails Legacy App Maintenance: 60min\r\nA World Without HackerNews: 30min\r\nUser Interface CSS in Rails Apps: 30min', [Validators.required]],
      tracks: [2, [Validators.required]],
    });

    this._meetingService = meetingService;
  }

  public Get(): void {
    console.log(this.meetingForm.value);
    this.getMeeting$ = this._meetingService.Post(this.meetingForm.value)
            .subscribe(
                p => {
                    this.output = p.output;
                    console.log(p.output);
                },
                e => {
                  this.output = e;
                  console.log(e);
              }
            );
  }
}

export interface Result {
  output?: string;
}
