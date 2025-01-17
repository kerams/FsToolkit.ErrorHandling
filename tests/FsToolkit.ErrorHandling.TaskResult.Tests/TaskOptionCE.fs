module TaskOptionCETests 

open Expecto
open FsToolkit.ErrorHandling
open System.Threading.Tasks
open FSharp.Control.Tasks

let makeDisposable () =
    { new System.IDisposable
        with member this.Dispose() = () }

[<Tests>]
let ceTests = 
    testList "Task CE" [
        testCaseTask "Return value" <| task {
            let expected = Some 42
            let! actual = taskOption  {
                return 42
            }
            Expect.equal actual expected "Should return value wrapped in option"
        }
        testCaseTask "ReturnFrom Some" <| task {
            let expected = Some 42
            let! actual = taskOption  {
                return! (Some 42)
            }
            Expect.equal actual expected "Should return value wrapped in option"
        }
        testCaseTask "ReturnFrom None" <| task {
            let expected = None
            let! actual = taskOption  {
                return! None
            }
            Expect.equal actual expected "Should return value wrapped in option"
        }

        testCaseTask "ReturnFrom Async None" <| task {
            let expected = None
            let! actual = taskOption  {
                return! (async.Return None)
            }
            Expect.equal actual expected "Should return value wrapped in option"
        }
        testCaseTask "ReturnFrom Async" <| task {
            let expected = Some 42
            let! actual = taskOption  {
                return! (async.Return 42)
            }
            Expect.equal actual expected "Should return value wrapped in option"
        }
        testCaseTask "ReturnFrom Task None" <| task {
            let expected = None
            let! actual = taskOption  {
                return! (Task.FromResult None)
            }
            Expect.equal actual expected "Should return value wrapped in option"
        }
        testCaseTask "ReturnFrom Task Generic" <| task {
            let expected = Some 42
            let! actual = taskOption  {
                return! (Task.FromResult 42)
            }
            Expect.equal actual expected "Should return value wrapped in option"
        }
        testCaseTask "ReturnFrom Task" <| task {
            let expected = Some ()
            let! actual = taskOption  {
                return! Task.CompletedTask
            }
            Expect.equal actual expected "Should return value wrapped in option"
        }
        testCaseTask "ReturnFrom ValueTask Generic" <| task {
            let expected = Some 42
            let! actual = taskOption  {
                return! (ValueTask.FromResult 42)
            }
            Expect.equal actual expected "Should return value wrapped in option"
        }
        testCaseTask "ReturnFrom ValueTask" <| task {
            let expected = Some ()
            let! actual = taskOption  {
                return! ValueTask.CompletedTask
            }
            Expect.equal actual expected "Should return value wrapped in option"
        }
        testCaseTask "Bind Some" <| task {
            let expected = Some 42
            let! actual = taskOption {
                let! value = Some 42
                return value
            }
            Expect.equal actual expected "Should bind value wrapped in option"
        }
        testCaseTask "Bind None" <| task {
            let expected = None
            let! actual = taskOption {
                let! value = None
                return value
            }
            Expect.equal actual expected "Should bind value wrapped in option"
        }
        testCaseTask "Bind Async None" <| task {
            let expected = None
            let! actual = taskOption {
                let! value = async.Return(None)
                return value
            }
            Expect.equal actual expected "Should bind value wrapped in option"
        }
        testCaseTask "Bind Async" <| task {
            let expected = Some 42
            let! actual = taskOption {
                let! value = async.Return 42
                return value
            }
            Expect.equal actual expected "Should bind value wrapped in option"
        }
        testCaseTask "Bind Task None" <| task {
            let expected = None
            let! actual = taskOption {
                let! value = Task.FromResult None
                return value
            }
            Expect.equal actual expected "Should bind value wrapped in option"
        }
        testCaseTask "Bind Task Generic" <| task {
            let expected = Some 42
            let! actual = taskOption {
                let! value = Task.FromResult 42
                return value
            }
            Expect.equal actual expected "Should bind value wrapped in option"
        }
        testCaseTask "Bind Task" <| task {
            let expected = Some ()
            let! actual = taskOption {
                let! value = Task.CompletedTask
                return value
            }
            Expect.equal actual expected "Should bind value wrapped in option"
        }
        testCaseTask "Bind ValueTask Generic" <| task {
            let expected = Some 42
            let! actual = taskOption {
                let! value = ValueTask.FromResult 42
                return value
            }
            Expect.equal actual expected "Should bind value wrapped in option"
        }
        testCaseTask "Bind ValueTask" <| task {
            let expected = Some ()
            let! actual = taskOption {
                let! value = ValueTask.CompletedTask
                return value
            }
            Expect.equal actual expected "Should bind value wrapped in option"
        }
        testCaseTask "Zero/Combine/Delay/Run" <| task {
            let data = 42
            let! actual = taskOption {
                let result = data
                if true then ()
                return result
            }
            Expect.equal actual (Some data) "Should be ok"
        }
        testCaseTask "Try With" <| task {
            let data = 42
            let! actual = taskOption {
                try 
                    return data
                with e -> 
                    return raise e
            }
            Expect.equal actual (Some data) "Try with failed"
        }
        testCaseTask "Try Finally" <| task {
            let data = 42
            let! actual = taskOption {
                try 
                    return data
                finally
                    ()
            }
            Expect.equal actual (Some data) "Try with failed"
        }
        testCaseTask "Using null" <| task {
            let data = 42
            let! actual = taskOption {
                use d = null
                return data
            }
            Expect.equal actual (Some data) "Should be ok"
        }
        testCaseTask "Using disposeable" <| task {
            let data = 42
            let! actual = taskOption {
                use d = makeDisposable ()
                return data
            }
            Expect.equal actual (Some data) "Should be ok"
        }
        testCaseTask "Using bind disposeable" <| task {
            let data = 42
            let! actual = taskOption {
                use! d = (makeDisposable () |> Some)
                return data
            }
            Expect.equal actual (Some data) "Should be ok"
        }
        testCaseTask "While" <| task {
            let data = 42
            let mutable index = 0
            let! actual = taskOption {
                while index < 10 do
                    index <- index + 1
                return data
            }
            Expect.equal actual (Some data) "Should be ok"
        }
        testCaseTask "For in" <| task {
            let data = 42
            let! actual = taskOption {
                for i in [1..10] do
                    ()
                return data
            }
            Expect.equal actual (Some data) "Should be ok"
        }
        testCaseTask "For to" <| task {
            let data = 42
            let! actual = taskOption {
                for i = 1 to 10 do
                    ()
                return data
            }
            Expect.equal actual (Some data) "Should be ok"
        }
    ]

[<Tests>]
let ceTestsApplicative =
    testList "TaskOptionCE applicative tests" [
        testCaseTask "Happy Path Option/AsyncOption/Ply/ValueTask" <| task {
            let! actual = taskOption {
                let! a = Some 3
                let! b = Some 1 |> Async.singleton
                let! c = Unsafe.uply { return Some 3 }
                let! d = ValueTask.FromResult (Some 5)
                return a + b - c - d
            }
            Expect.equal actual (Some -4) "Should be ok"
        }
        testCaseTask "Fail Path Option/AsyncOption/Ply/ValueTask" <| task {
            let! actual = taskOption {
                let! a = Some 3
                and! b = Some 1 |> Async.singleton
                and! c = Unsafe.uply { return None }
                and! d = ValueTask.FromResult (Some 5)
                return a + b - c - d
            }
            Expect.equal actual None "Should be ok"
        }
    ]

