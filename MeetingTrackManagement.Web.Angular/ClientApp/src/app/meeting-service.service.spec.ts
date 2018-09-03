import { TestBed, inject } from '@angular/core/testing';

import { MeetingService } from './meeting-service.service';

describe('MeetingServiceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MeetingService]
    });
  });

  it('should be created', inject([MeetingService], (service: MeetingService) => {
    expect(service).toBeTruthy();
  }));
});
