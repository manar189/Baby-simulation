clc
close all

runtime = 10;       % time of simulation [s]
h = 0.01;           % step size

x0 = 0.0;           % initial x-position
y0 = -0.3;          % initial y-position
v0 = 0.0;           % initial velocity
theta = pi/4;       % initial angle of motion
angle = 0;          % initial angle offset
w0 = 0.0;           % initial rotation

kickAvgR = 0.4;     % right leg average number of kicks per second
kickAvgL = 0.4;     % left leg average number of kicks per second
kickForceR = 1000;  % [N]
kickForceL = 1000;  % [N]

N = runtime / h;
uR = (rand(1, N) < kickAvgR / (N/runtime)) * kickForceR;
uL = (rand(1, N) < kickAvgL / (N/runtime)) * kickForceL;
u = [uR; uL];

z0 = [x0, v0*cos(theta), y0, v0*sin(theta), angle, w0];
[t, z] = RK4(@babyMotion, u, z0, N, h);

kickR = find(uR);
kickL = find(uL);
plot(t, z(1,:), t, z(3,:), t, z(5,:));
hold on
plot(kickR*h, z(1,kickR), 'k^', kickL*h, z(1,kickL), 'ko');
plot(kickR*h, z(3,kickR), 'k^', kickL*h, z(3,kickL), 'ko');
title('Position of the kicking baby');
legend('x position', 'y position', 'angle', 'right kick', 'left kick', 'Location', 'best');
nKicks = [length(kickR), length(kickL)]

createMovie('RK1', z, h)




