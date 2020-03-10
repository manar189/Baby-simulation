function dz = babyMotion(u, z)

% accelerations
[ax, ay, alpha] = babyAccelerations(z(1), z(2), z(3), z(4), z(5), z(6), u); 
dz = [z(2); ax; z(4); ay; z(6); alpha];

end

