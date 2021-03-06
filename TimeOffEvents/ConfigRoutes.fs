module TimeOff.Routes

open Suave
open Suave.Filters
open Suave.Operators
open System
open Commands

let parseHalfDay hd = match hd with
                        | "AM" -> AM
                        | "PM" -> PM

let app = 
    choose
      [ GET >=> choose 
      [ 
        //* User *
        //Route: user ask for a paid leave.
        pathScan "/userPaidLeaveAsk/%d/%d/%d/%d/%s/%d/%d/%d/%s" (fun(id, yb, mb, db, hdb, ye, me, de, hde) -> 
          let startDateHalfDay = parseHalfDay hdb
          let endDateHalfDay = parseHalfDay hde
          addHoliday id { Date = DateTime(yb, mb, db); HalfDay = startDateHalfDay } { Date = DateTime(ye, me, de); HalfDay = endDateHalfDay })
        //Route: user can list his own paid leave.
        pathScan "/userPaidLeaves/%d" (fun(id) -> getHoliday id)
        //Route: user cancel a pay leave.
        pathScan "/userPaidLeaveCancel/%d/%s" (fun(id, guid) -> cancelHoliday id guid)        
        //* Admin *
        //Route: admin accepts a pay leave for a user.
        pathScan "/adminPaidLeaveAccept/%d/%s" (fun(id, guid) -> acceptHoliday id guid) 
        //Route: admin refuses a pay leave for a user.
        pathScan "/adminPaidLeavesRefuse/%d/%s" (fun(id, guid) -> cancelHoliday id guid)
        //Route: admin cancel a pay leave for a user.
        pathScan "/adminPaidLeavesCancel/%d/%s" (fun(id, guid) -> cancelHoliday id guid) 
        //Route: admin lists all pay leaves
        pathScan "/adminPaidLeaves/%d" (fun(id) -> getHoliday id) ]]
