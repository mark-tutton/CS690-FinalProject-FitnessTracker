# User Documentation

A console-based application for planning and tracking workout routines.

## Running The Application

Follow the steps in the [Deployment guide](https://github.com/mark-tutton/CS690-FinalProject-FitnessTracker/wiki/Documentation#deployment-documentation) to get the application up and running in your local environment.

## Usage

After running `donet run` (or `dotnet FitnessTracker` if using the release version) in the project directory, you will be presented with the application's menu interface and able to interact with the application by selecting options from the menu.

```
 Welcome! No user selected

Main Menu
  > Create User
    Select User
    Exercises Library
    Workout Routines Library
    Start Workout
    View Workout History
    View Progress and Stats
    Exit
```

## Features

### 1. Creating / Selecting a User

**Create User**

- Enter a username.
- The new user is automatically set as the current user.

**Select User**

- Displays a list of existing users.
- Use the arrow keys to select a user. Press enter to confirm.

---

### 2. Exercise Library

Exercises are the building blocks of workout routines.

**Add Exercise**

1. Enter a name.
2. Select a type using the arrow keys and press enter to confirm:
   - Cardio
   - Strength
   - Flexibility
   - Balance
3. Depending on the type, you will be prompted for details:
   - **Strength** — enter number of sets and reps (e.g. 3 sets x 10 reps)
   - **Cardio** — enter distance (e.g. 2 miles) and duration (e.g. 30 mins). Optionally add interval sets and reps if it is an interval workout.
   - **Flexibility / Balance** — enter duration (e.g. 15 mins)

**Remove Exercise**

- A list of existing exercises is shown.
- Use the arrow keys to select the exercise to remove. Press enter to confirm.

**View Exercises**

- Displays all existing exercises in the library.

---

### 3. Workout Routines Library

Routines group exercises that can be completed as part of a workout session.

**Create Workout Routine**

1. Enter a name.
2. List of existing exercises is shown.
3. Use the arrow keys to navigate and space to select the exercises you want to include. Press enter to confirm.

**Remove Workout Routine**

- A list of existing routines is shown.
- Use the arrow keys to select the routine to remove. Press enter to confirm.

**View Workout Routines**

- Displays list of routines and their exercises.

---

### 4. Starting a Workout

1. Select a routine from the list.
2. Each exercise in the routine is shown with its prescribed details.
3. Log your actual performance for each exercise:
   - **Strength** — enter sets and reps completed
   - **Cardio** — enter distance (miles) and duration (mins). If the exercise has intervals, also enter interval sets and reps.
   - **Flexibility / Balance** — enter duration completed
4. Enter `y` to mark the session as completed.
5. Optionally enter the date/time of the workout (`mm/dd/yyyy hh:mm`), or leave blank to use the current date/time.

The session is saved to your workout history.

---

### 5. Viewing Workout History

Displays all recorded workout sessions with date, status, and logged exercise data.

---

### 6. Viewing Progress and Stats

Displays an aggregated stats table grouped by workout routine.

For each routine, the following totals and per-session averages are shown:

- Sessions completed
- Total sets and average sets per session
- Total reps and average reps per session
- Total minutes and average minutes per session
- Total distance and average distance per session

### 7. Comparing Workout Sessions

Compare the two most recent sessions of a workout routine to view progress.

**Navigation:** Main Menu → View Progress and Stats → Compare Recent Activities

1. Select an exercise type: Cardio, Strength, Flexibility, or Balance.
2. The app finds the two most recent completed sessions that contain exercises of the selected type.
3. A table is displayed comparing aggregated totals from each session:

| Metric        | Most Recent [date] | Previous [date] | Change |
| ------------- | ------------------ | --------------- | ------ |
| Sets          | 10                 | 9               | +1     |
| Reps          | 120                | 108             | +12    |
| Minutes       | 45.0               | 40.0            | +5.0   |
| Distance (mi) | 3.1                | 2.8             | +0.3   |

- Only rows with recorded data for at least one session are shown.
- If fewer than two sessions exist for the selected type, a message is shown and you are returned to the menu.

---

# Development Documentation

## Setup

### Clone the repo

```
git clone https://github.com/mark-tutton/CS690-FinalProject-FitnessTracker.git
cd CS690-FinalProject-FitnessTracker
```

### Build the project

```
cd CS690FinalProjectFitnessTracker/FitnessTracker
dotnet build
```

### Run the app

```
dotnet run
```

---

## Tests

Tests are located in the FitnessTracker.Tests directory.

### Run Tests

In the CS690FinalProjectFitnessTracker directory:

Run `dotnet test`

---

# Deployment Documentation

## Download the Release

1. Go to the [releases page](https://github.com/mark-tutton/CS690-FinalProject-FitnessTracker/releases)
2. Download the latest release
3. Extract the zip
4. cd into the FitnessTracker-v3.0.1 directory
5. Run the executable using `dotnet FitnessTracker.dll`
